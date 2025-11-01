function getCartCookieName() {
    return "cart-items-" + window.currentAccountId;
}

function addToCart(id, sessionPrice, sessionNumber, classId, classDay, classStartTime, classEndTime, professorFullName, courseName) {
    const cookieName = getCartCookieName(); 
    let sessions = $.cookie(cookieName);
    if (sessions === undefined) {
        sessions = [];
    } else {
        sessions = JSON.parse(sessions);
    }

    const currentProduct = sessions.find(x => x.id === id);
    if (currentProduct !== undefined) {
        return;
    } else {
        const session = {
            id,
            sessionPrice,
            sessionNumber,
            classId,
            classDay,
            classStartTime,
            classEndTime,
            professorFullName,
            courseName
        };

        sessions.push(session);
    }

    $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
    updateCart();
}

function updateCart() {
    const cookieName = getCartCookieName(); 
    let sessions = $.cookie(cookieName);
    if (sessions === undefined) {
        $(".cart_items_count").text(0);
        return;
    }
    sessions = JSON.parse(sessions);
    $(".cart_items_count").text(sessions.length);
}

function removeFromCart(id) {
    const cookieName = getCartCookieName(); 
    let sessions = $.cookie(cookieName);
    if (sessions === undefined) return;
    sessions = JSON.parse(sessions);
    const itemToRemove = sessions.findIndex(x => x.id === id);
    if (itemToRemove > -1) {
        sessions.splice(itemToRemove, 1);
        $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
    }
    updateCart();
}



//const cookieName = "cart-items";

//function addToCart(id, sessionPrice,sessionNumber,classId,classDay, classStartTime, classEndTime,professorFullName, courseName) {
//    let sessions = $.cookie(cookieName);
//    if (sessions === undefined) {
//        sessions = [];
//    } else {
//        sessions = JSON.parse(sessions);
//    }

//    const count = $("#productCount").val();
//    const currentProduct = sessions.find(x => x.id === id);
//    if (currentProduct !== undefined) {
//        sessions.find(x => x.id === id).count = parseInt(currentProduct.count) + parseInt(count);
//    } else {
//        const session = {
//            id,
//            sessionPrice,
//            sessionNumber,
//            classId,
//            classDay,
//            classStartTime,
//            classEndTime,
//            professorFullName,
//            courseName
//        }

//        sessions.push(session);
//    }

//    $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
//    updateCart();
//}

//function updateCart() {
//    let sessions = $.cookie(cookieName);
//    sessions = JSON.parse(sessions);
//    $(".cart_items_count").text(sessions.length);
//}

//function removeFromCart(id) {
//    let sessions = $.cookie(cookieName);
//    sessions = JSON.parse(sessions);
//    const itemToRemove = sessions.findIndex(x => x.id === id);
//    sessions.splice(itemToRemove, 1);
//    $.cookie(cookieName, JSON.stringify(sessions), { expires: 7, path: "/" });
//    updateCart();
//}


