declare @order_no nvarchar(10) = ''
		,@source_code_no int = 708
		,@front_counter char(1) = 'Y'

update top(1) oe_hdr
set source_code_no = @source_code_no 
	,front_counter = @front_counter
where order_no = @order_no