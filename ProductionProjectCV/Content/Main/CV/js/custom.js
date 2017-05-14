/*
Fancy Resume / CV 
Created: 02/02/2010
By: Catalin Pinte
Contact Info: pinte_catalin@yahoo.com
*/
$(document).ready(function(){

/* Fancy Box */
	$(".fancy-box").fancybox({

		  'overlayColor': "#111", 
		  'overlayOpacity': 0.7,
		  'easingIn' : 'easeInBack',
		  'easingOut' : 'easeOutBack'
	});
	
	$("#contact-me").fancybox({

		    'easingIn' : 'easeInBack',
		    'easingOut' : 'easeOutBack',
	        'padding' : 40,
			'overlayColor': "#111", 
			'overlayOpacity': 0.7,
			'scrolling'		: 'no',
			'titleShow'		: false
     });
	
/* Cufon font replacement */
	 Cufon.replace('h1');
	 Cufon.replace('h2');
	 Cufon.replace('h3');
	 Cufon.replace('h4');

	 Cufon.replace('.top-description');
	 Cufon.now();
		
	 var $query = '',
	     $elem = $("#tooltip");
	


    $("#background-switcher a").click(function(){
         var $backgr = $(this).parent().css('background');
		 $("body").css({
		     'background': $backgr,
			 'background-attachment':'fixed',
			 'background-position':'0 0'
		 });
	});

     $(".controls li").not('#tooltip').hover(
          function () {
		      clearTimeout($query);  
			  if($elem.attr("class") === 'active'){
				  $elem.clearQueue().animate({'left': ($(this).position().left - 38 ) + 'px'}, 300);
				  $elem.text($(this).attr('title'));
			  }
			  if ($elem.attr("class") === 'inactive'){
				  $elem.css({'left':  ($(this).position().left - 38 )+ 'px'}).css({'top': ($(this).position().top + 40)  + 'px'}).fadeIn("slow").attr('class','active');
				  $elem.html($(this).attr('title'));
		      }
	  
         }, 
		 function () {
			 $query = setTimeout( function(){ $elem.fadeOut("fast", function(){$elem.css({'left':'0'}).attr('class','inactive'); }); } ,500);
		}
    );
	 
    $(".controls li a").click(function(){
	     $elem.fadeOut("fast", function(){$elem.css({'left':'0'}).attr('class','inactive'); });
	});
	
	$(".controls li a#print").click(function(){
	     $elem.fadeOut("fast", function(){$elem.css({'left':'0'}).attr('class','inactive'); });
		 window.print();
		 return false;
	});
	

	$('#middle-content ul.list>li:nth-child(even)').not('.arrow-up').css({ 'background': '#e6e6e6'});
	$('#middle-content ul.list>li:nth-child(odd)').not('.arrow-up').css({ 'background': '#f6f6f6'});
	$('#middle-content ul.list:last-child').css({'margin-bottom':'0'});

	
/* Contact form validation */
     $("#contact").validate({
			debug: true,
			errorElement: "font",
			errorContainer: $("#warning, #summary"),
			errorPlacement: function(error, element) {
				error.appendTo( element.parent());
				element.addClass("error");
			},
			success: function(label) {
				label.text("This field is ok !").addClass("success");
			},
			rules: {
				firstname: {
				   required:true,
				   minlength:2,
				   maxlength:20
				},
				lastname: {
				   required:true,
				   minlength:2,
				   maxlength:20
				},
				message: {
				   required:true,
				   minlength:10,
				   maxlength:500
				},
				phone: {
					required:true,
					number:true,
					minlength:10,
					maxlength:14
				},
				email: {
				    required: true,
					email:true
				}
			}, 
			submitHandler: function(form) {
				    $("#send").attr("disabled","disabled");
					$("#loading").fadeIn();
                    $.ajax({
						   type: "POST",
						   url: "process_form.php", // php file with mai function 
						   data: $("#contact").serialize(),   
						   success: function(msg){
							   
							   $(':input','#contact').not(':button, :submit, :reset, :hidden').val('').removeAttr('checked').removeAttr('selected');						   
							   $("#contact font").hide().removeClass("success").removeClass("error");
						 
							   if(msg == "ok"){ $(".contact_success").slideDown("fast");  /* succes div */ } 
							   else {  $(".contact_error").slideDown("fast");  /* error div */ } 
							   
							   $("#send").attr("disabled","");
							   $("#loading").fadeOut("fast"); 	
							   
							}
							 
					});
			}
      });
	
});