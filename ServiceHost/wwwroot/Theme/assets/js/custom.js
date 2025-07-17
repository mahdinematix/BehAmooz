const cookieName = "cart-items";

function addToCart(id, sessionPrice,sessionNumber,classDay, classStartTime, classEndTime,professorFullName, courseName) {
    let sessions = $.cookie(cookieName);
    if (sessions === undefined) {
        sessions = [];
    } else {
        sessions = JSON.parse(sessions);
    }

    const count = $("#productCount").val();
    const currentProduct = sessions.find(x => x.id === id);
    if (currentProduct !== undefined) {
        sessions.find(x => x.id === id).count = parseInt(currentProduct.count) + parseInt(count);
    } else {
        const session = {
            id,
            sessionPrice,
            sessionNumber,
            classDay,
            classStartTime,
            classEndTime,
            professorFullName,
            courseName
        }

        sessions.push(session);
    }

    $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
    updateCart();
}

function updateCart() {
    let sessions = $.cookie(cookieName);
    sessions = JSON.parse(sessions);
    $(".cart_items_count").text(sessions.length);
}

function removeFromCart(id) {
    let sessions = $.cookie(cookieName);
    sessions = JSON.parse(sessions);
    const itemToRemove = sessions.findIndex(x => x.id === id);
    sessions.splice(itemToRemove, 1);
    $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
    updateCart();
}