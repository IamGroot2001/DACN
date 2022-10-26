window.addEventListener('DOMContentLoaded', event => {

    //Nhấn để ẩn hiện thanh navbar bên trái
    const sidebarToggle = document.body.querySelector('#sidebarToggle');
    if (sidebarToggle) {
        sidebarToggle.addEventListener('click', event => {
            event.preventDefault();
            document.body.classList.toggle('sidenav-toggled');
            localStorage.setItem('sidebar-toggle', document.body.classList.contains('sidenav-toggled'));
        });
    }

    //load giao diện bảng hóa đơn
    const datatablesInvoice = document.getElementById('datatablesInvoice');
    if (datatablesInvoice) {
        new simpleDatatables.DataTable(datatablesInvoice);
    }
    //load giao diện chi tiết bảng hóa đơn
    const datatablesInvoiceDetail = document.getElementById('datatablesInvoiceDetail');
    if (datatablesInvoiceDetail) {
        new simpleDatatables.DataTable(datatablesInvoiceDetail);
    }
    //load giao diện bảng khách hàng
    const datatablesCustomer = document.getElementById('datatablesCustomer');
    if (datatablesCustomer) {
        new simpleDatatables.DataTable(datatablesCustomer);
    }
    //load giao dien bảng loại đồ
    const datatablesTypesClothes = document.getElementById('datatablesTypesClothes');
    if (datatablesTypesClothes) {
        new simpleDatatables.DataTable(datatablesTypesClothes);
    }
    //load giao dien bảng sản phẩm
    const datatablesProduct = document.getElementById('datatablesProduct');
    if (datatablesProduct) {
        new simpleDatatables.DataTable(datatablesProduct);
    }
    //load giao dien bảng feedback
    const datatablesFeedback = document.getElementById('datatablesFeedback');
    if (datatablesFeedback) {
        new simpleDatatables.DataTable(datatablesFeedback);
    }
  
});


//in hóa đơn
$('#btnPrintBill').click(function () {
    window.print();
})
// hết in hóa đơn

