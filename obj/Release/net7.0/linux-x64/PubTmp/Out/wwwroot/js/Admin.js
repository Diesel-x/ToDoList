var deleteComment = (commentId) => {
    fetch("/api/Admin/DeleteComment?" + new URLSearchParams({
        commentId: commentId
    }), { method: 'POST' })
        .then(res => {
        if (res.status == 200) location.reload();
    });
}

var deletePost = (postId) => {
    fetch("/api/Admin/DeletePost?" + new URLSearchParams({
        postId: postId
    }), { method: 'POST' })
        .then(res => {
            if (res.status == 200) location.reload();
        });
}


$("[data-btndelcomment]").each(function () {
    $(this).click(() => {
        var comment = $(this).data("btndelcomment");
        deleteComment(comment);
    });
});


$("[data-btndelpost]").each(function () {
    $(this).click(() => {
        var post = $(this).data("btndelpost");
        deletePost(post);
    });
});

