function clearDialog() {
    $("#txtOrderIDDialog").val('');
    $('#txtProductIDDialog').val('');
    $("#txtUnitPriceDialog").val('');
    $("#txtQuantityDialog").val('');
    $("#txtDiscountDialog").val('');
}

function getExamData() {
    $.ajax({
        url: "/Exam/GetData",
        type: "POST",
        data: {
            pOrderID: $("#txtOrderID").val()
        },
        dataType: "json",
        //contentType: "application/json",
        success: function (result) {
            $("#txtOrderID").val(result[0].OrderID);
            $("#txtOrderDate").val(result[0].OrderDate);
            $("#txtCustomerID").val(result[0].CustomerID);
            $("#txtEmployeeID").val(result[0].EmployeeID);
            $("#txtShipName").val(result[0].ShipName);
            $("#txtShipAddress").val(result[0].ShipAddress);

            //dataTableData();
            tableItem = $('#tableItem').DataTable({
                fixedHeader: true,
                scrollY: "160px",
                scrollCollapse: true,
                paging: false,          //分頁
                searching: false,       //搜尋
                pageLength: false,      //一頁所顯示的資料數
                select: false,           //資料列的選取功能
                fixedColumns: false,
                info: false,           //顯示第幾頁
                destroy: true,
                ordering: false,      //排序
                processing: false,
                data: result,
                columnDefs: [
                    {
                        render: function (data, type, row) {
                            var ProductID = row.ProductID;

                            return "<div>" +
                                ProductID +
                                "</div>";

                        },
                        targets: [0],
                        orderable: false,
                    },
                    {
                        render: function (data, type, row) {
                            var UnitPrice = row.UnitPrice;
                            return "<div>" +
                                UnitPrice +
                                "</div>";
                        },
                        targets: [1],
                        orderable: false,
                    },
                    {
                        render: function (data, type, row) {
                            var Quantity = row.Quantity;
                            return "<div>" +
                                Quantity +
                                "</div>";
                        },
                        targets: [2],
                        orderable: false,
                    },
                    {
                        render: function (data, type, row) {
                            var Discount = row.Discount;
                            return "<div>" +
                                Discount +
                                "</div>";
                        },
                        targets: [3],
                        orderable: false,
                    },
                    {
                        render: function (data, type, row) {
                            var ProductID = row.ProductID;
                            return "<div class='text-center'>" +
                                //"<input type='button' id='del" + ProductID + "' name='del" + ProductID + "' width='18px' height='18px' style='cursor: pointer' value='刪除' />" +
                                "<a id='del" + ProductID + "' name='del" + ProductID + "' href='#'>刪除</a>" +
                                    "</div>";
                        },
                        width: "4%",
                        targets: [4],
                        orderable: false,
                    }
                ],
                createdRow: function (row, data, dataIndex) {
                },
                rowCallback: function (row, data, index) {
                }
            });
        },
        error: function (e) {
            alert(e.responseText);
        }
    });
}

function updateOrdersDetail(vJsonString) {
    $.ajax({
        url: "/Exam/UpdateOrdersDetail",
        type: "POST",
        dataType: "json",
        data: {
            jsonList: vJsonString
        },
        success: function (result) {
            alert(result);
            getExamData();
        },
        error: function (e) {
            alert(e.responseText);
        }
    });
}


$(function () {
    $("#ExamDialog").dialog({
        title: "Orders Detail",
        autoOpen: false,
        modal: true,
        height: 850,
        width: 750,
        dialogClass: "dlg-no-close",
        buttons: {
            OK: function () {
                $(this).dialog("close");
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        },
        close: function () {
            
        }
    });

    $("#tableItem tbody").on("mouseover", "tr", function (event) {
        $(this).children('td:eq(0)').css("background-color", "#EBD3E8");
        $(this).children('td:eq(1)').css("background-color", "#EBD3E8");
        $(this).children('td:eq(2)').css("background-color", "#EBD3E8");
        $(this).children('td:eq(3)').css("background-color", "#EBD3E8");
        $(this).css('cursor', 'pointer');
    });

    $("#tableItem tbody").on("mouseleave", "tr", function (event) {
        $(this).children('td:eq(0)').css("background-color", "#FFFFFF");
        $(this).children('td:eq(1)').css("background-color", "#FFFFFF");
        $(this).children('td:eq(2)').css("background-color", "#FFFFFF");
        $(this).children('td:eq(3)').css("background-color", "#FFFFFF");
        $(this).css('cursor', 'none');
    });


    $("#tableItem tbody").on("click", "td", function (event) {
        var idx = $(this).index();
        var sProductId = $(this).parents('tr').children('td:eq(0)').text();

        if (idx == 4) {
            if (confirm("Confirm to delete?")) {
                $.ajax({
                    url: "/Exam/DelOrdersDetail",
                    type: "POST",
                    dataType: "json",
                    data: {
                        pOrderID: $("#txtOrderID").val(),
                        pProductID: sProductId
                    },
                    success: function (result) {
                        alert(result);
                        getExamData();
                    },
                    error: function (e) {
                        alert(e.responseText);
                    }
                });
            }
        } else {
            $.ajax({
                url: "/Exam/GetDetail_Dialog",
                type: "POST",
                dataType: "json",
                data: {
                    pOrderID: $("#txtOrderID").val(),
                    pProductID: sProductId
                },
                success: function (result) {
                    $("#txtOrderIDDialog").val(result[0].OrderID);
                    $("#txtProductIDDialog").val(result[0].ProductID);
                    $("#txtUnitPriceDialog").val(result[0].UnitPrice);
                    $("#txtQuantityDialog").val(result[0].Quantity);
                    $("#txtDiscountDialog").val(result[0].Discount);
                },
                error: function (e) {
                    alert(e.responseText);
                }
            });

            var dialog = $("#ExamDialog").dialog({
                title: "Orders Detail",
                autoOpen: false,
                modal: true,
                height: 650,
                width: 550,
                dialogClass: "dlg-no-close",
                buttons: {
                    Submit: function () {
                        var jsonString = "[{'OrderID': '" + $("#txtOrderIDDialog").val() + "', " +
                            "'ProductID': '" + $("#txtProductIDDialog").val() + "', " +
                            "'UnitPrice': '" + $("#txtUnitPriceDialog").val() + "', " +
                            "'Quantity': '" + $("#txtQuantityDialog").val() + "', " +
                            "'Discount': '" + $("#txtDiscountDialog").val() + "'}]";

                        if (jsonString !== "") {
                            updateOrdersDetail(jsonString);
                        }

                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                },
                close: function () {
                    clearDialog();
                    getExamData();
                }
            });

            $('#ExamDialog').dialog('open');
        }
    });


    $("#btnSubmit").click(function () {
        //var tableItem = null;
        getExamData();
    });
})

