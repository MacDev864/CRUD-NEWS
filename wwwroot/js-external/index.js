const LOADER_BODY = `
<div class="text-center">
    <div class="spinner-border" style="width: 3rem; height: 3rem;" role="status"></div>
    <div class="mt-2">Loading data...</div>
</div>
`;
const BTN_NOMAL = {
    show_btn: function () {
        return BTN_TEXT_NOMAL
    }
}
const BTN_TEXT_NOMAL = `
บันทึก
`
const APP_LOADING_DTB = {
    showLoading: function (colspan) {
        return `
        <tr>
            <td colspan="${colspan}">
                ${LOADER_BODY}
            </td>
        </tr>
        `;
    }
};
const BTN_TEXT_PROCESSING = `
<span class="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>กำลังบันทึกข้อมูล
`
const BTN_LOADING = {
    show_btn: function () {
        return BTN_TEXT_PROCESSING
    }
}
function fetchData() {
    let tbody = $('#Table-1 > tbody');
    tbody.empty().append(APP_LOADING_DTB.showLoading(4)); // Adjusted colspan value

    $.ajax({
        url: '/api/news/all',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                let tbodyString = '';
                if (response.data.length > 0) {
                    response.data.forEach(function (item) {
                        tbodyString += `
                            <tr>
                                <td width="130px">${item.name || ""}</td>
                                <td class="text-center" width="150px">
                                    <button type="button" class="btn btn-sm btn-view btn-outline-info"  data-id="${item.id} "><i class="fas fa-search"></i></button>
                                    <button type="button" class="btn btn-sm btn-edit btn-outline-warning" data-id="${item.id}"><i class="fas fa-edit"></i></button>
                                    <button type="button" class="btn btn-sm btn-delete btn-outline-danger" data-id="${item.id}"><i class="fas fa-trash-alt"></i></button>
                                </td>
                            </tr>
                        `;
                    });
                } else {
                    tbodyString = `
                        <tr>
                            <td colspan="2" class="text-center">ไม่พบข้อมูล!</td>
                        </tr>
                    `;
                }
                tbody.empty().append(tbodyString);
                if ($.fn.dataTable.isDataTable("#Table-1")) {
                    $("#Table-1").DataTable().destroy();
                }

                var table = $("#Table-1").DataTable({
                    "responsive": false,
                    "lengthChange": true,
                    "autoWidth": true,
                    searching: true,
                    buttons: [
                        {
                            extend: 'copyHtml5',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4]
                            }
                        },
                        {
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4]
                            }
                        },
                        {
                            extend: 'print',
                            exportOptions: {
                                columns: [0, 1, 2, 3, 4]
                            },
                            title: $('#print').data('pagename'),
                            customize: function (win) {
                                $(win.document.body).css('font-size', '10pt');
                                $(win.document.body).find('table').addClass('compact').css('font-size', 'inherit');
                            },
                        },
                    ],
                    drawCallback: function () {
                        initEvent();
                        return false;
                    },
                });

                $('#print').append(table.buttons().container());
                $('.dt-buttons.btn-group.flex-wrap').addClass('mt-2 mb-2');
            }
        },
      
    });
}

function renderDataTable(tbody, tbodyString) {
    
}

function fetchByID(id, type) {
    $('#form-name').text("แก้ไขข้อมูล")

    $.ajax({
        url: '/api/news/' + id,
        type: 'GET',
        success: function (response) {
            if (response.success) {
                if (type == "view") {
                    window.location.href = "/News/Detail/" + id;
                }
                if (type == "edit") {
                    $('#id').val(response.data.id)
                    $('#name').val(response.data.name)
                    $('#description').val(response.data.description)
                    $('#modal-01').modal('show')
                } else {
                    $('#id').val(response.data.id)
                    $('#name').val(response.data.name)
                    $('#description').val(response.data.description)
                    $('#modal-01').modal('show')

                }
            

            }
        },
        error: function (xhr, status, error) {
            console.error('Error: ', error);
        }
    });
}
function eventDestroy(id) {
    fetchByID(id)
    disabled()
    $('#frm-modal-1').data('frmStatus', 'destroy')
    $('.btn-submit').text('ลบข้อมูล')
}
function disabled() {
    $("#name").prop("disabled", "disabled");
}
function initEvent() {
    $('.btn-view').click(function () {

       let id = $(this).data("id");
        eventview(id);
    });
    $('.btn-edit').click(function () {

        let id = $(this).data("id");
        eventEdit(id)
    });
    $(".btn-delete").off().on("click", function (event) {
        let id = $(this).data("id");
        eventDestroy(id)
    });
}

function eventAdd() {
    $('#modal-01').modal('show');
    $('#form-name').text("บันทึกข้อมูล")
    $('#frm-modal-1').data('frmStatus', 'create')
    restFormValue()
}

function eventview(id) {
    fetchByID(id,"view");
    // Assuming disabled() function is defined elsewhere
}
function eventEdit(id) {
    fetchByID(id,"edit")
    $('#frm-modal-1').data('frmStatus', 'edit')
}
function restFormValue() {
    let datafrm = {
        id: $('#id').val(''),
        name: $('#name').val(''),
        description: $('#description').val(''),
        img: $('#img').val(''),
        //gobal valiable
    }
    return datafrm
}
function getFormValue() {
    let datafrm = {
        id: $('#id').val(),
        name: $('#name').val(),
        description: $('#description').val(),
        img: $('#img').val(),
        //gobal valiable
    }
    return datafrm
}
function create(url, type, frmValue) {
    $('.btn-submit').attr('disabled', 'disabled');
    $('.btn-submit').html(BTN_LOADING.show_btn());
    $.ajax({
        url: url,
        type: type,
        contentType: 'application/json', // Content type
     data: JSON.stringify(frmValue),
        success: function (response) {
            $('.btn-submit').html(BTN_NOMAL.show_btn());
            $('.btn-submit').removeAttr('disabled');
            $("#modal-01").modal("hide");
            window.location.href = "/";
        }
    });
}
function update(url, type, frmValue) {
    $('.btn-submit').attr('disabled', 'disabled');
    $('.btn-submit').html(BTN_LOADING.show_btn());
    $.ajax({
        url: url,
        type: type,
        data: frmValue,
        contentType: 'application/json', // Content type
        data: JSON.stringify(frmValue),

        success: function (response) {

            $('.btn-submit').html(BTN_NOMAL.show_btn());
            $('.btn-submit').removeAttr('disabled');
            $("#modal-01").modal("hide");
            window.location.href = "/";
        }
    });
}
function destroy(url, type, frmValue) {
    $('.btn-submit').attr('disabled', 'disabled');
    $('.btn-submit').html(BTN_LOADING.show_btn());
    $.ajax({
        url: url,
        type: type,
        data: frmValue,
        contentType: 'application/json', // Content type
        data: JSON.stringify(frmValue),
        success: function (response) {
            $('.btn-submit').html(BTN_NOMAL.show_btn());
            $('.btn-submit').removeAttr('disabled');
            $("#modal-01").modal("hide");
            window.location.href = "/";
        }
    });
}
$(document).ready(function () {
    fetchData();

    $('.btn-add').click(function () {
        eventAdd();
    });
    $('.modal-btn').click(function () {
        let dismiss = $(this).data("dismiss");
        if (dismiss == "modal") {
            $("#modal-01").modal("hide");
            window.location.href = "/";
        }
    });
  
    // --------------------------------------------------
    //  submit
    // --------------------------------------------------
    $('#frm-modal-1').on('submit', function (event) {
        event.preventDefault();

        let frmStatus = $('#frm-modal-1').data('frmStatus');
        let frmValue = getFormValue();
        let url = '/api/news';
        let type = '';

        if (this.checkValidity() != false) {

            if (frmStatus == "create") {
                url = '/create';
                type = 'post';
               create(url, type, frmValue);
            } else if (frmStatus == "edit") {

                url += '/update';
                type = 'post';
                update(url, type, frmValue)
            } else {
                url += '/delete';
                type = 'post';
                destroy(url, type, frmValue);
            }
        }
    });




    // --------------------------------------------------
    //  set-modal
    // --------------------------------------------------
    $("#modal-01").on('hidden.bs.modal', function () {
    });
});
