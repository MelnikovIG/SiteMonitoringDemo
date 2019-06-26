$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/monitoringEvent")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.start().then(function () {
        console.log("connected");
    });

    connection.on("ReceiveMonitoringData",
        (data) => {
            clearAll();

            for (var i = 0; i < data.length; i++) {
                addNew(data[i]);
            }
        });

    function clearAll() {
        $("table tbody").remove();
    }

    function getStatusCell(status) {
        switch (status) {
        case 0: return "<td class='bg-warning text-white'>Unknown</td>";
        case 1: return "<td class='bg-success text-white'>Online</td>";
        case 2: return "<td class='bg-danger text-white'>Offline</td>";
        default: return "<td>Undefined</td>";
    }}
    
    function addNew(data) {
        var row = '<tr rowId="' + data.id + '">' +
            '<td id="uri">' + data.uri + '</td>' +
            '<td id="refresh-time">' + data.refreshTimeInSeconds + '</td>' +
            getStatusCell(data.status) +
            '<td>' + actions + '</td>' +
            '</tr>';
        $("table").append(row);
    }

    function validateModalData() {
        var uriValue = $('#editInfoModal #uri').val();
        if (!uriValue) {
            alert("Uri should be filled");
            return false;
        }

        var refreshTime = $('#editInfoModal #refresh-time').val();
        if (!refreshTime) {
            alert("RefreshTime should be filled");
            return false;
        }

        if (refreshTime <= 0) {
            alert("RefreshTime should be positive");
            return false;
        }

        return true;
    }

    $('[data-toggle="tooltip"]').tooltip();
    var actions = '<a class="edit" title="Edit" data-toggle="tooltip"><i class="material-icons"></i></a>\r\n<a class="delete" title="Delete" data-toggle="tooltip"><i class="material-icons">';

    // Append table with add row form on add new button click
    $(".add-new").click(function () {

        $('#editInfoModal #uri').val(null);
        $('#editInfoModal #refresh-time').val(null);
        $('#editInfoModal').modal();

        $('#editInfoModal #editInfoModalSaveBtn').click(function () {

            if (!validateModalData()) {
                return;
            }

            var uriValue = $('#editInfoModal #uri').val();
            var refreshTime = $('#editInfoModal #refresh-time').val();

            connection
                .invoke("CreateRecord", uriValue, refreshTime)
                .catch(err => alert(err));


            $('#editInfoModal').modal('hide');
        });
    });

    // Edit row on edit button click
    $(document).on("click", ".edit", function () {
        var trElem = $(this).parents("tr");
        var id = trElem.attr("rowId");
        var uri = trElem.children('td#uri')[0].innerHTML;
        var refreshTime = trElem.children('td#refresh-time')[0].innerHTML;

        $('#editInfoModal #uri').val(uri);
        $('#editInfoModal #refresh-time').val(refreshTime);
        $('#editInfoModal').modal();

        $('#editInfoModal #editInfoModalSaveBtn').click(function () {

            if (!validateModalData()) {
                return;
            }

            var uriValue = $('#editInfoModal #uri').val();
            var refreshTime = $('#editInfoModal #refresh-time').val();

            connection
                .invoke("UpdateRecord", id, uriValue, refreshTime)
                .catch(err => alert(err));

            $('#editInfoModal').modal('hide');
        });
    });

    // Delete row on delete button click
    $(document).on("click", ".delete", function () {
        var trElem = $(this).parents("tr");
        var id = trElem.attr("rowId");

        connection
            .invoke("RemoveRecord", id)
            .catch(err => alert(err));

        trElem.remove();
        $(".add-new").removeAttr("disabled");
    });

    $('#editInfoModal').on('hidden.bs.modal', function (e) {
        $('#editInfoModal #editInfoModalSaveBtn').unbind("click");
    });
});