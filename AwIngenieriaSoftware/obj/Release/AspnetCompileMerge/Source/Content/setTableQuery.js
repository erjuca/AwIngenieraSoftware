$('#datos').DataTable({
    //"scrollY": "350px",
    "scrollX": true,
    "scrollCollapse": true,
    "order": [0, 'desc'],
    "lengthMenu": [[100, 200, 300, -1], [100, 200, 300, "Todos"]]
});
$('.dataTables_length').addClass('bs-select');

