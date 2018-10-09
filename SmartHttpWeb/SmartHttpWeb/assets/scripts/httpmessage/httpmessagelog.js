function handle() {
    var form = $("#form1");
    var error = $('.alert-danger', form);
    var success = $('.alert-success', form);

    form.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block', // default input error message class
        focusInvalid: true, // do not focus the last invalid input
        ignore: "",
        rules: {
            Name: {
                required: true
            },
            Desc: {
                required: true
            }
        },
        messages: {
            Name: {
                required: "请输入动作名称"
            },
            Desc: {
                required: "请输入描述",
            }
        },

        invalidHandler: function (event, validator) { //display error alert on form submit
            success.hide();
            error.show();
        },
        highlight: function (element) { // hightlight error inputs
            $(element)
            .closest('.form-group').addClass('has-error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change done by hightlight
            $(element)
            .closest('.form-group').removeClass('has-error'); // set error class to the control group
        },
        success: function (label) {
            label
            .closest('.form-group').removeClass('has-error'); // set success class to the control group
        },
        submitHandler: function (form) {
            success.show();
            error.hide();
        }

    });
}

function searchData(pageIndex) {
    pageIndex = pageIndex || 1;
    var rqData = $("#form1").ToMVCData();
    rqData.PageIndex = pageIndex;
    var pageContent = $('#divList');
    App.blockUI(pageContent, false);
    $.ajax({
        type: "POST",
        data: rqData,
        traditional: true,
        url: "/HttpMessage/HttpMessageLogListView",
        success: function (res) {
            App.unblockUI(pageContent);
            if (res != null && res != "") {
                $("#divList").html(res);
            }
        },
        error: function (res) {
            App.unblockUI(pageContent);
        }
    });
    return false;
}