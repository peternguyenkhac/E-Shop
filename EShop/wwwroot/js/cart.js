async function AddToCart(productId, quantity) {
    let formData = new FormData();
    formData.append("productId", productId);
    formData.append("quantity", quantity);

    fetch("/Cart/AddToCart", {
        method: "POST",
        redirect: "follow",
        body: formData
    })
        .then(response => {
            if (response.redirected) {
                window.location.href = response.url;
            } else if (response.ok) {
                alert("Đã thêm sản phẩm vào giỏ hàng");
            } else {
                alert("Lỗi");
            }
        })
        .catch(error => {
            alert(error);
        });

}

function UpdateSubToTalPrice() {
    var elements = document.querySelectorAll(".sub-price");
    var sum = 0; // initialize the sum
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i]; // get the current element
        var content = element.innerHTML; // or element.innerText
        var number = Number(content.match(/\d+/g)[0]); // get the first number in the content
        sum += number; 
    }
    document.getElementById("sub-total").innerHTML = sum + " VND";
}

UpdateSubToTalPrice();

function UpdateCartItem(productId, quantity) {
    if (quantity == 0) {
        RemoveCartItem(productId);
    } else {
        let formData = new FormData();
        formData.append("productId", productId);
        formData.append("quantity", quantity);

        fetch("/Cart/UpdateCartItem", {
            method: "POST",
            redirect: "follow",
            body: formData
        })
            .then(response => {
                if (response.ok) {
                    var row = document.getElementById(productId);
                    var price = Number(row.querySelector(".origin-price").innerHTML.match(/\d+/g)[0]);
                    row.querySelector(".sub-price").innerHTML = price * quantity + " VND";
                    UpdateSubToTalPrice();
                } else {
                    alert("Lỗi");
                }
            })
            .catch(error => {
                alert(error);
            });
        
    }
    
}

function RemoveCartItem(productId) {
    let formData = new FormData();
    formData.append("productId", productId);

    fetch("/Cart/RemoveCartItem", {
        method: "POST",
        redirect: "follow",
        body: formData
    })
        .then(response => {
            if (response.ok) {
                var row = document.getElementById(productId);
                row.remove();
                alert("Đã xoá sản phẩm");
            } else {
                alert("Lỗi");
            }
        })
        .catch(error => {
            alert(error);
        });
    UpdateSubToTalPrice();
}


async function AddProductToCart(productId) {
    var quantity = document.getElementById("product-quantity").value;
    await AddToCart(productId, quantity);
}
