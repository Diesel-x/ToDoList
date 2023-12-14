var send_comment = (id) => {
    fetch("/api/Comments/Create?" + new URLSearchParams({
        relatedPostId: id,
        Text: $(`[data-inppostid=${id}]`).val(),
    }), { method: 'POST' }).then(res => {
        if (res.status == 200) location.reload();
    });
};

$("[data-btnpostid]").each(function () {
    $(this).click(() =>
    {
        var id = $(this).data("btnpostid");
        send_comment(id);
    });
});

$("[data-inppostid]").each(function () {
    $(this).on('keypress',(e) => {
        if (e.which == 13) {
            var id = $(this).data("inppostid");
            send_comment(id);
        }
    })
});