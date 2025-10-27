/* DARK MODA */
var $dark = $(".header-wrapper");

if (localStorage.getItem("darkmode") === "true") {
  $dark.addClass("dark-header");
  $(
    ".price-item--regular,p,h1,h2,h3,h4,h5,h6,.h1, .h2, .h3, .h4, .h5, .h6,ul,li,a,span,address,.footer__copyright.copyright__content a:hover,.footer__copyright,.header__icons.header__icon svg,.header__inline-menu.list-menu--inlineli.header__menu-item.header__active-menu-item, .header__inline-menu.list-menu--inlineli.header__menu-item,.header-wrapper.header.header__heading span,.card-wrapper.card__content.card-information.price, .card-wrapper.card__content.card__heading,.announcement-bar .announcement-bar__message, .icon-caret, .form__label, .banner__text, .product-form__input, .accordion .icon-accordion, .cart-notification-product__name, .cart-items, .caption-with-letter-spacing, .localization-selector.link, .product-tab-wrapper .collection .tabs .tablinks, .product__info-wrapper .share-button__button, .product__info-container .varient-model-wrapper .size-chart, .flash-sale-banner .copy-text input.text, .flash-sale-banner .copy-text button, .facet-filters__field>label, .dt-sc-product_bundle-items .bundle-product-offer-note, .cart-item__price-wrapper .price, .tax-note, .facets__form-vertical, .product-grid-container"
  ).addClass("light-text");
  $(
    ".color-background-2, .video-section__media, .gradient, #cart-notification-button, .header-wrapper .header, .disclosure__list-wrapper , ul.dt-nav li>.megamenu_megamenu>div.sub-menu-block, ul.dt-nav>li .megamenu_megamenu.mega-menu>div.sub-menu-block .dt-sc--main-menu--mega>ul.mega-menu-brands .dt-sc-menu-image-with-text .dt-sc-mega_menu .dt-sc-mega_menu-link h4, .multicolumn-list__item .multicolumn-card, .customer-who-purchased .product-data, .newsletter-modal .newsletter-content, .collection-list.overlay .collection-list__item .card__inner, .article-template__comment-wrapper, .flex-banner .image-bar__section-inner .image-bar__item, .isotope-products .isotope-selector .card, .hotspot-block .dt-sc-hotspot-item, .insta-gallery-section.overlay .insta-gallery-wrapper .insta-gallery-content, #MainContent .sticky-bar, .dt-sc-product_bundle-items .bundle-product-additional-offer, .product-popup-modal__content, .facet-filters.sorting .facets__display, .facets__display, .facets__header, .detial-login, .customer:not(.account):not(.order), .cart-drawer .drawer__inner, .cart__footer"
  ).addClass("dark-background");
  $(".darkmode-button").addClass("darkmode-button-light");
  $(".light-logo").addClass("logo-hide");
  $(".dark-logo").addClass("logo-display");
}

$(".darkmode-button").click(function () {
  $dark.toggleClass("dark-header");
  $(
    ".price-item--regular,p,h1,h2,h3,h4,h5,h6,ul,li,a,span,address,.footer__copyright.copyright__content a:hover,.footer__copyright,.header__icons.header__icon svg,.header__inline-menu.list-menu--inlineli.header__menu-item.header__active-menu-item, .header__inline-menu.list-menu--inlineli.header__menu-item,.header-wrapper.header.header__heading span,.card-wrapper.card__content.card-information.price, .card-wrapper.card__content.card__heading,.announcement-bar .announcement-bar__message, .icon-caret, .form__label, .banner__text, .product-form__input, .accordion .icon-accordion, .cart-notification-product__name, .cart-items, .caption-with-letter-spacing, .localization-selector.link, .product-tab-wrapper .collection .tabs .tablinks, .product__info-wrapper .share-button__button, .product__info-container .varient-model-wrapper .size-chart, .flash-sale-banner .copy-text input.text, .flash-sale-banner .copy-text button, .facet-filters__field>label, .dt-sc-product_bundle-items .bundle-product-offer-note, .cart-item__price-wrapper .price, .tax-note, .facets__form-vertical, .product-grid-container"
  ).toggleClass("light-text");
  $(
    ".color-background-2, .video-section__media, .gradient, #cart-notification-button, .header-wrapper .header, .disclosure__list-wrapper, ul.dt-nav li>.megamenu_megamenu>div.sub-menu-block, ul.dt-nav>li .megamenu_megamenu.mega-menu>div.sub-menu-block .dt-sc--main-menu--mega>ul.mega-menu-brands .dt-sc-menu-image-with-text .dt-sc-mega_menu .dt-sc-mega_menu-link h4, .multicolumn-list__item .multicolumn-card, .customer-who-purchased .product-data, .newsletter-modal .newsletter-content, .collection-list.overlay .collection-list__item .card__inner, .article-template__comment-wrapper, .flex-banner .image-bar__section-inner .image-bar__item, .isotope-products .isotope-selector .card, .hotspot-block .dt-sc-hotspot-item, .insta-gallery-section.overlay .insta-gallery-wrapper .insta-gallery-content, #MainContent .sticky-bar, .dt-sc-product_bundle-items .bundle-product-additional-offer, .product-popup-modal__content, .facet-filters.sorting .facets__display, .facets__display, .facets__header, .detial-login, .customer:not(.account):not(.order), .cart-drawer .drawer__inner, .cart__footer"
  ).toggleClass("dark-background");
  $(".darkmode-button").toggleClass("darkmode-button-light");
  $(".light-logo").toggleClass("logo-hide");
  $(".dark-logo").toggleClass("logo-display");

  localStorage.setItem("darkmode", $dark.hasClass("dark-header"));
});

$("#mode-switch").click(function () {
  if (localStorage.getItem("darkmode") === "true") {
    $("#mode-switch").text("Light Mode");
  } else {
    $("#mode-switch").text("Dark Mode");
  }
});

$(document).ready(function () {
  if (localStorage.getItem("darkmode") === "true") {
    $("#mode-switch").text("Light Mode");
  } else {
    $("#mode-switch").text("Dark Mode");
  }
});
