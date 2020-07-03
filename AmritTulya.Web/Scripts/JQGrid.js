$(function () {
    //    debugger;

    $.fn.fmatter.imageFormatter = function (cellvalue, options, rowObject) {
        return "<img src='~/Content/UploadImages/" + cellvalue + "'/>";
    };


    $("#grid").jqGrid
        ({
            url: "/Inventory/GetValues",
            datatype: 'json',
            mtype: 'Get',
            //table header name
            colNames: ['Id', 'Name', 'Description', 'Price', 'Image'],
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
                    multiline: true,
                    height: '60px'

                }, {
                    key: false,
                    name: 'Price',
                    index: 'Price',
                    editable: true,
                    align : 'center'
                },
                {
                    name: "ImagePath",
                    index: "ImagePath",
                    editable: true,
                    editrules: { required: true },
                    edittype: "file",
                    search: false,
                    resizable: false,
                    width: 210,
                    align: "center",
                    editoptions: {
                        enctype: "multipart/form-data"
                    },
                    formatter: imageFormat
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
                afterSubmit: uploadImage,
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
                afterSubmit: uploadImage,
                afterComplete: function (response) {
                    if (response.responseText) {
                        if (response.responseText == "Saved Successfully") {
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

});


function imageFormat(cellvalue, options, rowObject) {
    if (cellvalue != null) {
        return '<img src="Content/UploadImages/' + cellvalue + '" style="height:75px !important;width:200px !important" />';
    }
    return '<img src="#" style="height:0px !important;width:0px !important" />';
}
function imageUnFormat(cellvalue, options, cell) {
    return $('img', cell).attr('src');
}



//function uploadImage(response, postdata) {
//    if (response.statusText == "OK") {
//        if ($("#ImagePath").val() != "") {
//           var result= ajaxFileUpload(postdata.Id);
//        }
//        else {
//            return "Added successfully";
//        }
//    }
//}

function uploadImage(response, postdata) {
    //debugger;
    //var json = $.parseJSON(response.responseText);
    //if (json) return [json.success, json.message, json.id];
    //return [false, "Failed to get result from server.", null];
    debugger;
    //var data = $.parseJSON(response.responseText);

    if (response.status == 200) {
        if ($("#ImagePath").val() != "") {
            ajaxFileUpload(postdata.Id);
        }
    }
    return [response.status, response.responseText, postdata.Id];
}

function ajaxFileUpload(id) {
    var data = new FormData();
    var files = $("#ImagePath").get(0).files;
   //if (files.length > 0) {
        data.append("MyImages", files[0]);
   //}

    $.ajax({
        url: "/Inventory/UploadImage?id=" + id,
        type: "POST",
        processData: false,
        contentType: false,
        data: data,
        success: function (response) {
            return response;
          },
        //error: function (er) {
        //    alert(er);
        //}

    });
}
