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
                    $("selector").click(false);
                    var that = this;
                    let $input = $("input", this.header());

                    $input.click(false); // prevent sorting when clicked
                    $input.on("keyup change clear ", function () {
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });
                });

        },
        ...customDatatableOptions
    };

    return $tableEl.DataTable(dataTableOptions);
};