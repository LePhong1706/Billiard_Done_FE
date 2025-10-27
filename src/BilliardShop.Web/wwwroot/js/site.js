function addToCart(productId, quantity) {
    if (!quantity) quantity = 1;

    $.ajax({
        url: '/Cart/AddToCart',
        type: 'POST',
        data: {
            productId: productId,
            quantity: quantity
        },
        success: function (response) {
            if (response.success) {
                // Update cart drawer if exists
                if ($('#CartDrawer-CartItems').length) {
                    $('#CartDrawer-CartItems').html(response.html);
                }

                // Update cart count bubble if exists
                if ($('.cart-count-bubble').length) {
                    $('.cart-count-bubble span[aria-hidden="true"]').text(response.count);
                    $('.cart-count-bubble .visually-hidden').text(response.count + ' sản phẩm');
                }

                // Update subtotal if exists
                if ($('.totals__subtotal-value').length) {
                    $('.totals__subtotal-value').text(response.subTotal.toLocaleString('vi-VN') + ' ₫');
                }

                // Show success message
                alert(response.message || 'Đã thêm sản phẩm vào giỏ hàng!');

                // Try to show cart drawer if exists
                if ($('cart-drawer').length) {
                    $('cart-drawer').addClass('active');
                    $('body').addClass('overflow-hidden-tablet');
                }
            } else {
                alert(response.message || 'Không thể thêm sản phẩm vào giỏ hàng');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error adding to cart:', error);
            alert('Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng');
        }
    });
}