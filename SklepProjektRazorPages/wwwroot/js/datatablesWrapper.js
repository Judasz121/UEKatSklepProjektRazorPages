function GenerateDataTable($tableEl, customDatatableOptions) {
    $tableEl = $tableEl instanceof jQuery ? $tableEl : $($tableEl);
    let dataTableOptions = {


        // search inputs on column headers
        dom: "ltipr",
        searching: true,
        initComplete: function (settings) {
            
            this.api()
                .columns()
                .every(function () {
                    var that = this;
                    let $input = $("input", this.header());

                    $input.click(false); // prevent sorting when clicked
                    $input.on("keyup change clear ", function () {
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });

                    $input.on('keydown', e => { // ctrl+a fix
                        if (e.keyCode == 65 && e.ctrlKey) {
                            e.currentTarget.select();
                        }
                    })
                });

        },
        ...customDatatableOptions
    };

    return $tableEl.DataTable(dataTableOptions);
};