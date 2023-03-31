using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SklepProjektRazorPages.DbModels;
using SklepProjektRazorPages.Helpers;
using System.Data;
using Dapper;
using System.Dynamic;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Linq;

namespace SklepProjektRazorPages.Pages.AdminPanel.Category
{
    public class ViewEditModel : PageModel
    {
        public List<Kategoria> categories = new List<Kategoria>();
        public void OnGet()
        {
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                categories = conn.Query<Kategoria>("SELECT * FROM Kategoria").ToList();

            }
        }

        public IActionResult OnPostUpdateTree(List<jsTreeNode> nodes)
        {
            dynamic result = new ExpandoObject();
            result.success = true;
            result.errors = new List<(string title, string message)>();
            List<jsTreeNode> nodesForDeleteSection  = new List<jsTreeNode>(nodes);
            List<int> newlyAddedCategoriesId = new List<int>();
            
            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                conn.Open();                
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                    #region addition and update
                    List<string> alreadyHandledNewNodesId = new List<string>();
                    void handleNodeNewParentNode(jsTreeNode child)
                    {
                        jsTreeNode parent = nodes.First(pn => pn.id == child.parent);
                        if (parent.parent.StartsWith("j1_"))
                            /*parent.parent = */handleNodeNewParentNode(parent);
                        alreadyHandledNewNodesId.Add(parent.id);
                        int newId = conn.ExecuteScalar<int>("INSERT INTO Kategoria (Nazwa, ID_Rodzica) VALUES (@text, @parent); SELECT SCOPE_IDENTITY()",
                            parent, trans);


                        IEnumerable<jsTreeNode> nodesWithSameParent = nodes.Where(n => n.parent == child.parent);
                        foreach (jsTreeNode node in nodesWithSameParent)
                            node.parent = newId.ToString();
                        child.parent = newId.ToString();
                        //return newId.ToString();
                    }
                    foreach (jsTreeNode node in nodes)
                    {
                        if (alreadyHandledNewNodesId.Contains(node.id))
                            continue;
                        if (string.IsNullOrWhiteSpace(node.parent) || node.parent == "#")
                            node.parent = null;
                        else if (node.parent.StartsWith("j1_"))
                            /*node.parent = */handleNodeNewParentNode(node);                       

                        if (node.id.StartsWith("j1_"))
                        {// add new node
                            int newId = conn.ExecuteScalar<int>("INSERT INTO Kategoria (Nazwa, ID_Rodzica) VALUES (@text, @parent); SELECT SCOPE_IDENTITY()",
                                node, trans);
                            newlyAddedCategoriesId.Add(newId);
                            // update parent's id in its children
                            IEnumerable<jsTreeNode> nodeChildren = nodes.Where(n => n.parent == node.id);
                            foreach (jsTreeNode child in nodeChildren)
                                child.parent = newId.ToString();
                        }
                        else
                        {// update existing node
                            int rowsAff = conn.Execute("UPDATE Kategoria SET ID_Rodzica = @parent, Nazwa = @text WHERE ID_Kategorii = @id",
                                node, trans);
                        }
                    }
                    #endregion addition and update
                    #region deletion
                    List<Kategoria> dbCategories = conn.Query<Kategoria>("SELECT * FROM Kategoria", null, trans).ToList();
                    List<int> notDeletedCategoriesId = nodesForDeleteSection.Where(node => int.TryParse(node.id, out int result)) // <- elimination of new ids ( those start with "j_1" )
                        .Select(node => int.Parse(node.id)).ToList();
                    notDeletedCategoriesId = notDeletedCategoriesId.Concat(newlyAddedCategoriesId).ToList();
                    List<int> deletedCategoriesId = dbCategories.Select(c => (int)c.ID_Kategorii).Except(notDeletedCategoriesId).ToList();
                    
                    void deleteChildren(int parentId)
                    {
                        List<Kategoria> children = dbCategories.Where(cat => cat.ID_Rodzica == parentId).ToList();
                        if (children.Count == 0)
                            return;

                        string sql = "DELETE FROM Kategoria WHERE ";
                        for (int i = 0; i < children.Count(); i++)
                        {
                            Kategoria child = children[i];
                            deleteChildren((int)child.ID_Kategorii);
                            sql += $"ID_Kategorii = {child.ID_Kategorii}";
                            if (i < children.Count - 1)
                                sql += " OR ";
                        }
                        conn.Execute(sql, null, trans);

                    }
                    foreach (int delCatId in deletedCategoriesId)
                    {
                        deleteChildren(delCatId);
                        conn.Execute("DELETE FROM Kategoria WHERE ID_Kategorii = @id",
                            new { id = delCatId }, trans);
                    }


                    #endregion deletion
                    trans.Commit();
                }
                conn.Close();
            }

            return new JsonResult(result);
        }

        public IActionResult OnPostUpdateCategoriesParent(List<NodeParentUpdate> newParentNodes)
        {
            dynamic result = new ExpandoObject();
            result.success = true;
            result.errors = new List<(string title, string message)>();

            using (IDbConnection conn = DbHelper.GetDbConnection())
            {
                conn.Open();
                using (IDbTransaction trans = conn.BeginTransaction())
                {
                   
                    foreach (var categoryUpdate in newParentNodes)
                    {
                        if (categoryUpdate.newParentId == 0)
                            categoryUpdate.newParentId = null;
                        int rowsAff = conn.Execute("UPDATE Kategoria SET ID_Rodzica = @newParentId WHERE ID_Kategorii = @id", 
                            newParentNodes, trans);
                        //if (rowsAff != 1)
                        //{
                        //    trans.Rollback();
                        //    result.errors.Add(("Server Error:", "one category parent update didn't affect precisely one row."));
                        //    result.success = false;
                        //    break;
                        //}
                    }
                    trans.Commit();
                }
                conn.Close();
            }
            return new JsonResult(result);
        }
        public class NodeParentUpdate
        {
            public int id { get; set; }
            public int? newParentId { get; set; }
        }
        public class jsTreeNode
        {
            public string id { get; set; }
            public string parent{ get; set; }
            public string text { get; set; }
        }
    }

}
