var nhansuController = {
    init: function() {
        nhansuController.loadData();
        nhansuController.registerEvent();
    },
    registerEvent: function() {
        nhansuController.loadData();
        nhansuController.registerEvent();
    },
    loadData : function() {
        $.ajax({
            url: '/NhanSu/LoadData',
            type: 'GET',
            dataType: 'Json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ID: item.EMPLOYEE_ID,
                            Name: item.FULL_NAME,
                            Status: item.WORKSTATUS == true ? "<span class=\"label label-success\">Active</span>" : "<span class=\"label label-danger\">Lock</span>"
                        });
                    });
                    $('#tblData').html(html);
                    nhansuController.registerEvent();
                }
            }
        });
    }
}
nhansuController.init();