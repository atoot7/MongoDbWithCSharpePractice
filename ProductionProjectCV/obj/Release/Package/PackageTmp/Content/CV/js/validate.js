/*
!nteractive
Created: 12/01/2010
By: Catalin Pinte
Contact Info: pinte_catalin@yahoo.com
*/


				   
		  $("#contact_form").validate({
			debug: false,
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
							   
							   $(':input','#contact_form').not(':button, :submit, :reset, :hidden').val('').removeAttr('checked').removeAttr('selected');						   
							   $("#contact_form font").hide().removeClass("success").removeClass("error");
						 
							   if(msg == "ok"){ $(".contact_success").slideDown("fast");  /* succes div */ } 
							   else {  $(".contact_error").slideDown("fast");  /* error div */ } 
							   
							   $("#send").attr("disabled","");
							   $("#loading").fadeOut("fast"); 	
							   
							}
							 
					});
			}
      });
    
