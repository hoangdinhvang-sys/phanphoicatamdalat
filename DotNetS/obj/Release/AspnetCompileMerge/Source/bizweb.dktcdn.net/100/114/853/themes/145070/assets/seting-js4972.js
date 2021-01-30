$(document).ready(function() {
	$('#custom1').owlCarousel({
		autoplay:true,
		autoplayTimeout:5000,
		loop:true,
		dots:false,
		items:1,
		navText: [ 
			'<i class="fa fa-chevron-left" aria-hidden="true"></i>', 
			'<i class="fa fa-chevron-right" aria-hidden="true"></i>' 
		],
		autoHeight:false
	});

	$(".list-blogs").owlCarousel({
		slideSpeed: 300,
		paginationSpeed: 400,
		margin:30,
		autoplay:true,
    	autoplayTimeout:3000,
		loop:true,
		dots:false,
		responsive:{
			0:{
				items:1,
			},
			480:{
				items:2,
			},
			768:{
				items:2,
			},
			992:{
				items:1,
			}
		},
		navText: [ 
			'<span class="button-prev"><i class="fa fa-angle-left" aria-hidden="true"></i></span>', 
			'<span class="button-next"><i class="fa fa-angle-right" aria-hidden="true"></i></span>' 
		],
	});

	$(".owl-giamgia").owlCarousel({
		slideSpeed: 300,
		paginationSpeed: 400,
		items:1,
		dots:false,
		navText: [ 
			'<span class="button-prev"><i class="fa fa-angle-left" aria-hidden="true"></i></span>', 
			'<span class="button-next"><i class="fa fa-angle-right" aria-hidden="true"></i></span>' 
		],
	});

	$('.owl-sanphammoi').owlCarousel({
		loop:true,
		margin:30,
		dots:false,
		nav:false,
		autoplay:true,
    	autoplayTimeout:3000,
		responsiveClass:true,
		responsive:{
			0:{
				items:2,
			},
			544:{
				items:2,
			},
			768:{
				items:3,
			},
			992:{
				items:3,
			}
		}
	});
	
	$('.owl-sanphamlienquan').owlCarousel({
		loop:false,
		margin:30,
		dots:false,
		nav:false,
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
			},
			544:{
				items:2,
			},
			768:{
				items:4,
			},
			992:{
				items:4,
			}
		}
	});

	$('.owl-khuyenmai').owlCarousel({
		loop:true,
		margin:30,
		autoplay:true,
    	autoplayTimeout:3000,
		dots:false,
		navText: [ 
			'<span class="button-prev"><i class="fa fa-angle-left" aria-hidden="true"></i></span>', 
			'<span class="button-next"><i class="fa fa-angle-right" aria-hidden="true"></i></span>' 
		],
		responsiveClass:true,
		responsive:{
			0:{
				items:2,
			},
			544:{
				items:2,
			},
			768:{
				items:3,
			},
			992:{
				items:3,
			}
		}
	});

	$('.owl-noibat').owlCarousel({
		loop:true,
		margin:30,
		autoplay:true,
    	autoplayTimeout:3000,
		dots:false,
		navText: [ 
			'<span class="button-prev"><i class="fa fa-angle-left" aria-hidden="true"></i></span>', 
			'<span class="button-next"><i class="fa fa-angle-right" aria-hidden="true"></i></span>' 
		],
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
			},
			544:{
				items:2,
			},
			768:{
				items:2,
			},
			992:{
				items:1,
			}
		}
	});

	$(".owl-muanhieu").owlCarousel({
		loop:true,
		margin:30,
		autoplay:true,
    	autoplayTimeout:8000,
		dots:false,
		navText: [ 
			'<span class="button-prev"><i class="fa fa-angle-left" aria-hidden="true"></i></span>', 
			'<span class="button-next"><i class="fa fa-angle-right" aria-hidden="true"></i></span>' 
		],
		responsiveClass:true,
		responsive:{
			0:{
				items:1,
			},
			544:{
				items:1,
			},
			768:{
				items:2,
			},
			992:{
				items:2,
			}
		}
	});

});


jQuery(document).ready(function ($) {

	var $sync1 = $(".product-images"),
		$sync2 = $(".thumbnail-product"),
		flag = false,
		duration = 300;

	$sync1
		.owlCarousel({
		items: 1,
		margin: 10,
		nav: true,
		dots: false,
		navText: [ 
			'<i class="fa fa-chevron-left" aria-hidden="true"></i>', 
			'<i class="fa fa-chevron-right" aria-hidden="true"></i>' 
		],
	})
		.on('changed.owl.carousel', function (e) {
		if (!flag) {
			flag = true;
			$sync2.trigger('to.owl.carousel', [e.item.index, duration, true]);
			flag = false;
		}
	});

	$sync2
		.owlCarousel({
		margin: 20,
		items: 4,
		nav: false,
		dots: false,
	})
		.on('click', '.owl-item', function () {
		$sync1.trigger('to.owl.carousel', [$(this).index(), duration, true]);

	})
		.on('changed.owl.carousel', function (e) {
		if (!flag) {
			flag = true;		
			$sync1.trigger('to.owl.carousel', [e.item.index, duration, true]);
			flag = false;
		}
	});
});


$("#share").jsSocials({
	showLabel: false,
	showCount: false,
	shares: ["facebook", "twitter", "googleplus", "linkedin", "pinterest"]
});


$("#shareButtonLabelCount").jsSocials({
	showCount: true,
	showLabel: true,
	shares: ["twitter", "facebook", "googleplus", "linkedin", "pinterest", "stumbleupon", "whatsapp"]
});