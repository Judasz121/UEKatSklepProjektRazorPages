function GenerateDataTable($tableEl) {
    $tableEl = $tableEl instanceof jQuery ? $tableEl : $($tableEl);
    let dataTableOptions = {
        dom: "ltipr",
        searching: true,
        initComplete: function (settings) {
            //console.log("initComplete");
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
    };

    return $tableEl.DataTable(dataTableOptions);
};