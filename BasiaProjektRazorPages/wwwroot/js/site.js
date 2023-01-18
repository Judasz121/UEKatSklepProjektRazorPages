// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function Dropdown() {
    document.getElementById("PopupMenu").classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.drophover')) {

        var dropdowns = document.getElementsByClassName("dropMenu-container");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}
function GenerateDataTables(tableEl) {
    document.addEventListener('DOMContentLoaded', e => {
        $tableEl = $tableEl instanceof jQuery ? $tableEl : $($tableEl);
        let $tableEl = $("#accounts-table");
        let dataTableOptions = {
            dom: "ltipr",
            searching: true,
            initComplete: function (settings) {
                console.log("initComplete");
                this.api()
                    .columns()
                    .every(function () {
                        $("selector").click(false);
                        var that = this;
                        let $input = $("input", this.header());

                        $input.click(false); // prevent sorting when clicked
                        $input.on("keyup change clear", function () {
                            if (that.search() !== this.value) {
                                that.search(this.value).draw();
                            }
                        });
                    });

            },
        }
        $tableEl.DataTable(dataTableOptions);
    })

}

