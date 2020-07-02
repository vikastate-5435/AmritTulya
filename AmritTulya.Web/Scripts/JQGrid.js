$(function () {
    //    debugger;

    $.fn.fmatter.imageFormatter = function (cellvalue, options, rowObject) {
        return "<img src='/Shared/AuftragDBImage?id=" + cellvalue + "'/>";
    };


    $("#grid").jqGrid
        ({
            url: "/Inventory/GetValues",
            datatype: 'json',
            mtype: 'Get',
            //table header name
            colNames: ['Id', 'Name', 'Description', 'Price', 'InventoryImage'],
            //colModel takes the data from controller and binds to grid   
            colModel: [
                {
                    key: true,
                    hidden: true,
                    name: 'Id',
                    index: 'Id',
                    editable: true
                }, {
                    key: false,
                    name: 'Name',
                    index: 'Name',
                    editable: true
                }, {
                    key: false,
                    name: 'Description',
                    index: 'Description',
                    editable: true,
                    multiline: true

                }, {
                    key: false,
                    name: 'Price',
                    index: 'Price',
                    editable: true
                }, {
                    name: "InventoryImage",
                    index: "InventoryImage",
                    mtype: "Post",
                    editable: true,
                    editrules: { required: true },
                    edittype: "file",
                    search: true,
                    resizable: false,
                    width: 210,
                    align: "left",
                    editoptions: {
                        enctype: "multipart/form-data"
                    }

                }
            ],

            pager: jQuery('#pager'),
            rowNum: 10,
            rowList: [10, 20, 30, 40],
            height: '100%',
            viewrecords: true,
            caption: 'Sadguru Amrit Tulya! Tea Shop',
            emptyrecords: 'No records to display',
            jsonReader:
            {
                root: "rows",
                page: "page",
                total: "total",
                records: "records",
                repeatitems: false,
                Id: "0"
            },
            autowidth: true,
            multiselect: false
            //pager-you have to choose here what icons should appear at the bottom  
            //like edit,create,delete icons  
        }).navGrid('#pager',
            {
                edit: true,
                add: true,
                del: true,
                search: false,
                refresh: true
            }, {
                // edit option
                zIndex: 100,
                url: '/Inventory/Edit',
                closeOnEscape: true,
                closeAfterEdit: true,
                recreateForm: true,
               // afterSubmit: uploadImage,
                afterComplete: function (response) {
                    if (response.responseText) {
                        alert(response.responseText);
                    }
                }
            }, {
                // add options  
                zIndex: 100,
                url: "/Inventory/Create",
                closeOnEscape: true,
                closeAfterAdd: true,
                //  afterSubmit: uploadImage,
                afterComplete: function (response) {
                    if (response.responseText) {
                        if (response.responseText == "Saved Successfully") {
                            var data = response.responseText;
                    // alert(data.id);
                            alert(response.responseText);
                        }
                    }
                }
            }, {
                // delete options  
                zIndex: 100,
                url: "/Inventory/Delete",
                closeOnEscape: true,
                closeAfterDelete: true,
                recreateForm: true,
                msg: "Are you sure you want to delete this product?",
                afterComplete: function (response) {
                    if (response.responseText) {
                        alert(response.responseText);
                    }
                }
            });

    $("#InventoryImage").change(function () {

        var imagefilepath = $(this).val();
        if (this.files && this.files[0]) {
            var obj = new FileReader();
            obj.onload = function (data) {
                var image = document.getElementById("image");

                image.src = data.target.result;
                image.style.display = "block";

            }
            obj.readAsDataURL(this.files[0]);
        }
    })

});


function imageFormat(cellvalue, options, rowObject) {
    return '<img src="' + cellvalue + '" />';
}
function imageUnFormat(cellvalue, options, cell) {
    return $('img', cell).attr('src');
}



function uploadImage(response, postdata) {
    debugger;
    //var json = $.parseJSON(response.responseText);
    //if (json) return [json.success, json.message, json.id];
    //return [false, "Failed to get result from server.", null];

    //var data = $.parseJSON(response.responseText);

    if (response.statusText == "OK") {
        if ($("#InventoryImage").val() != "") {
            ajaxFileUpload(postdata.Id);
        }
    }

    return [data.success, data.message, data.id];
}

function ajaxFileUpload(id) {

    var formData = new FormData();
    var files = $("#InventoryImage").get(0).files;
    if (files.length > 0) {
        formData.append("MyImages", files[0]);
    }
    formData.append("Id", id);


    debugger;
    var imageData = JSON.stringify({
        'Id': id,
        'InventoryImage': files
    });

    debugger;
    $.ajax(
        {
            type: "POST",
            url: "/Inventory/UploadImage?id=" + id + "InventoryImage=" + files,
            secureuri: false,
            fileElementId: 'InventoryImage',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ formData }),
            success: function (response, status) {
                alert('hiii');
                debugger;
                if (typeof (response.error) != 'undefined') {
                    if (response.error != '') {
                        alert(response.error);
                    } else {
                        alert(response.msg);
                    }
                }
            },
            error: function (response, status, e) {
                alert(e);
            }
        }
    )

    //return false;
}
