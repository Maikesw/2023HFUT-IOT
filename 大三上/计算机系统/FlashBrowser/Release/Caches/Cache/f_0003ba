var last_div = "";
var vote_flag = 0;
var tday=new Date();
M=Number(tday.getMonth())+1
var now = tday.getFullYear()+"-"+M+"-"+tday.getDate()+ " "+tday.getHours()+":"+tday.getMinutes();

  function switch_div(id){
	var div1 = 'hide_'+id;
	var div2 = 'show_'+id;
	document.getElementById(div1).style.display="block";
	document.getElementById(div2).style.display="none";
  }  
  function s_t(time){
    var tday = LocatTime(new Date());
    var tmp = (Date.parse(new Date())/1000-86400)*1000;
    var yday = LocatTime(new Date(tmp));
    var pday = LocatTime(new Date(time*1000));
    var stday = tday.substring(0,tday.indexOf(" "));
    var syday = yday.substring(0,yday.indexOf(" "));
    var spday = pday.substring(0,pday.indexOf(" "));
    var ptime = pday.substr(pday.indexOf(" ")+1,5);
    var pdate = "";
    if (spday == stday)
        pdate = "今天";
    else if (spday == syday)
        pdate = "昨天";
    else
        pdate = spday;
    document.write(pdate+' '+ptime);
  }
  function LocatTime(d){
	var Y = d.getFullYear();
	var M = d.getMonth()+1;
	var D = d.getDate();
	var hh = d.getHours();
	var mm = d.getMinutes();
	if (M < 10) M = '0'+M;
	if (D < 10) D = '0'+D;   
	if (hh < 10) hh = '0'+hh;
	if (mm < 10) mm = '0'+mm;
	return Y+'/'+M+'/'+D+''+' '+hh+":"+mm;
  }
  function add_reply(fid,cid,in_bbsgames){
	if (pre_cid != 0){
		reply_b=document.getElementById('com_reply_add_' + pre_cid);
		reply_b.innerHTML='';	
	}
	pre_cid = cid;
    reply_b=document.getElementById('com_reply_add_' + cid);
	reply_b.innerHTML='<div class="lam" id="rptpl"><div class="hf2">'
+'<div class="exp">'
+'<a href="javascript:add_face2(\'01\');"><img title="发呆" src="/p2images/01.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'02\');"><img title="可爱" src="/p2images/02.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'03\');"><img title="憨笑" src="/p2images/03.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'04\');"><img title="坏笑" src="/p2images/04.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'05\');"><img title="冷汗" src="/p2images/05.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'06\');"><img title="发怒" src="/p2images/06.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'07\');"><img title="狂抓" src="/p2images/07.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'08\');"><img title="可怜" src="/p2images/08.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'09\');"><img title="委屈" src="/p2images/09.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'10\');"><img title="阴险" src="/p2images/10.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'11\');"><img title="晕" src="/p2images/11.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'12\');"><img title="擦汗" src="/p2images/12.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'13\');"><img title="困" src="/p2images/13.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'14\');"><img title="害羞" src="/p2images/14.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'15\');"><img title="惊慌" src="/p2images/15.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'16\');"><img title="坏笑" src="/p2images/16.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'17\');"><img title="色" src="/p2images/17.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'18\');"><img title="得意" src="/p2images/18.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'19\');"><img title="鬼" src="/p2images/19.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'20\');"><img title="囧" src="/p2images/20.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'21\');"><img title="睡觉" src="/p2images/21.gif" border="0"/></a>'
+'<a href="javascript:add_face2(\'22\');"><img title="调皮" src="/p2images/22.gif" border="0"/></a>'
+'</div>'
+'<form action="//cdn.comment.4399pk.com/?op=reply" target="hideFrame" method="post" onsubmit="return submit_reply(this)">'
+'<input type="hidden" name="fid" value="'+fid+'" />'
+'<input type="hidden" name="cid" value="'+cid+'" />'
+'<input type="hidden" name="anonymous" value="1" />'
+'<input type="hidden" name="reply" value="" />'
+'<input type="hidden" name="show_bbs_dialog" value="'+in_bbsgames+'">'
+'<textarea id="reply_block" name="reply_show" onpaste="return false;" cols="" rows=""></textarea>'
+'<div class="hf2_btn"><input class="btn1" name="" value="" type="submit" /><input class="btn2" name="" type="button" onclick="remove_reply('+cid+')"/></div>'
+'<span>提示：本站为防止低俗内容出现，用户发表的回复审核后才能显示出来,谢谢 ！</span>'
+'</form></div></div>';		
    setTimeout(function(){document.getElementById('reply_block').focus();},100);
    return false;
  }  
  function submit_reply(f){
    f.reply.value=f.reply_show.value;
    if(f.reply.value.length < 3){
        alert('回复内容不得小于5个字符！多写几个字吧～～');
        f.reply_show.focus();
        return false;
    }
	var new1 = f.reply.value;
	new1 = new1.replace(/\[FA:[0-9][0-9]\]/g,"");	
    if(new1.length < 3){
        alert('回复内容不得小于3个字符!回复中不能只有表情！');
        f.reply_show.focus();
        return false;
    }	
    var mycid = f.cid.value;
	var str= f.reply.value;
    str =str.replace(/script/ig,"");
    str =str.replace(/java/ig,"");
    str =str.replace(/</ig,"");
    str =str.replace(/>/ig,"");	
    var show_comment = str.replace(/\[FA:([0-9][0-9])\]/g,"<img src='/p2images/$1.gif'>");	
    document.getElementById('reply_add_to_this_'+mycid).innerHTML='<div class="hf1"><div class="hf_le"><div class="hf_wj"><b>我</b><span>4399玩家</span><em>回复于 '+now+'</em></div><p>'+show_comment+'</p></div><div class="hf_ri"><?=$cnt?></div><div class="clear"></div></div>';	
    f.reply_show.value='';
    return true;
  } 
  function remove_reply(cid){
    reply_b=document.getElementById('com_reply_add_' + cid);
	reply_b.innerHTML='';  
  }  
  function add_face(id){
    var o = document.getElementById('comment_block');
    var s = o.value;
	if (o.value=='在这里输入您的评论...') o.value='';
    var reg = new RegExp("FA:[0-9][0-9]", "gm");

    if (s.match(reg) == null)
        var a = 1;
    else
        if (s.match(reg).length >= 3) { alert("每个评论最多只能选择三个表情哦！");return;}
    o.value = o.value+"[FA:"+id+"]";
    o.focus();
  }  
  function add_face2(id){
    var o = document.getElementById('reply_block');
    if (o.value == '在这里输入您的回复...') o.value = '';	
    var s = o.value;
    var reg = new RegExp("FA:[0-9][0-9]", "gm");
    if (s.match(reg) == null)
        var a = 1;
    else
        if (s.match(reg).length >= 3) { alert("每个回复最多只能选择三个表情哦！");return;}
    o.value = o.value+"[FA:"+id+"]";
    o.focus();
  }
  function upflower(id){
    if (vote_flag == 0){
		var myid = 'top_'+id;
		var a = document.getElementById(myid).innerHTML;
		a = parseInt(a);  
		a++;
		document.getElementById(myid).innerHTML = a;
	    vote_flag = 1;
	    return true;
    }else{alert('您已投过票了，谢谢:)请明天再来吧！');
	    return false;
    }
  }  
  function chg_spn(id){
    var myid = 'spn_'+id;
    var a = document.getElementById(myid).innerHTML;
    a = parseInt(a);
    a++;
    document.getElementById(myid).innerHTML = a;
  }
  
  function disp_txt(){
    document.getElementById('dialog_txt').style.display='block';
  }
  function hide_txt(){
    document.getElementById('dialog_txt').style.display='none';
  }  
  var lan = new Array(10);
  lan[0]="这游戏真好玩．我喜欢．支持4399！";
  lan[1]="运气真好．又坐到沙发了！";
  lan[2]="游戏太简单，小菜一碟！随随便便就通关啦！";
  lan[3]="很好玩，我喜欢玩(*^__^*) 嘻嘻……";
  lan[4]="这游戏太好玩了，我们都把他打爆机了!!";
  lan[5]="游戏太难玩了，我过不去啊！";
  lan[6]="这游戏太好玩了，我玩了无数次了。";
  lan[7]="游戏一般般，不太好玩了！";
  lan[8]="这游戏好差哦。我一点也不喜欢。我踩。";
  lan[9]="游戏玩不了呀，怎么回事！！";

  function add_text(id){
    var flag = 0;
    var o = document.getElementById('comment_block');
    if (o.value == '在这里输入您的评论...') o.value = '';
    for (var i = 0;i <=9;i++){
        var idx = o.value.indexOf(lan[i]);
        if ( idx != -1){
            flag = 1;
            break;
        }
    }
    if ( flag == 1)
        alert("你已经选择了一条快捷发言哦，每次回复只能包含一条快捷信息！");
    else{
        o.value = o.value+lan[id];
    }
    document.getElementById('dialog_txt').style.display="none";
    o.focus();
  }
  function show_text(){
	document.writeln('<ul>');
    for (var i = 0;i <=9;i++)
        document.writeln('<li><a href="javascript:add_text('+i+');">'+lan[i]+'</a></li>');
	document.writeln('</ul>');
  }
  
  function show_msg(msg){
	document.getElementById('msg_txt').innerHTML=msg;
	showWin('msg_dialog');
	setTimeout(closeWin,2000);	
	return false;
  }
  function submit_comment(f){
    f.comment.value=f.comment_show.value;
    var blk=document.getElementById('comment_block');
	var str = f.comment.value;
	var new1 = str.replace(/\[FA:[0-9][0-9]\]/g,"");
	if (new1.length < 5){ alert('评论内容不得少于10个字符');blk.focus();return false;}
    if(f.comment.value==''||new1=='' || f.comment.value=='在这里输入您的评论...'){
		alert('请填写评论内容！');
        blk.focus();
        return false;
    }
    var str=f.comment.value;
    str =str.replace(/script/ig,"");
    str =str.replace(/java/ig,"");
    str =str.replace(/</ig,"");
    str =str.replace(/>/ig,"");		
    var show_comment = str.replace(/\[FA:([0-9][0-9])\]/g,"<img src='/p2images/$1.gif'>");
    document.getElementById('add_to_this').innerHTML='<div class="lam"><div class="lam_t"><div class="wj"><b>我</b><span>4399玩家</span><em>发表于 '+now+'</em></div><div class="star"></div></div><div class="tex"><p>'+show_comment+'</p></div></div>';
    f.comment_show.value='在这里输入您的评论...';
	return true;
  }
  
  function resize_frame(){
    var h=document.getElementById('FrameHeight').offsetHeight;
    try{
        parent.ResizeFrame('CommFrame', h);
    }catch(e){}
    document.getElementById('loading').style.display='none';
  }
  
  function frame_reload(){
    window.location=window.location.toString();
  }
  
  function my_detail(id){
	document.getElementById('hidden_div_'+id).style.display="block";
	document.getElementById('show_detail_'+id).style.display="none";
  }
  function add_comment(){
    var blk=document.getElementById('goto');
    setTimeout(function(){blk.focus();}, 100);
    return false;  
  }
  function  get_para(n){
	var ua = "&"+window.location.search.substring(1) + "&";
	var un = "&"+n+"=";
	var ul = un.length;
	var ui = ua.indexOf(un);
	if(ui == -1)return '';
    if ((ua.indexOf('.4399.com') <0) && (ua.indexOf('.5054399.com') <0) && (ua.indexOf('.4399pk.com') <0))
        return 'www.4399.com';	
	var ue = ua.indexOf("&", ui+ul);
	return ua.substring(ui+ul, ue);

}  
  
  function goto_page(fid,tag){
     var t_p = parseInt(document.getElementById('goto').value);
	 var p_url = '';
	 if (t_p > pages) t_p = pages;
	 if (t_p < 1) t_p = 1;
	 var ua2 = window.location.href;	 
	 if (ua2.indexOf('jf_') < 0)
		p_url = fid+'.htm?abc=1';
	 else	 
		p_url = 'jf_'+fid+'.htm?popup=yes';
     if (t_p > 1){
		if (tag == 'hot')
			p_url = '/nhot-'+fid+'-'+t_p+'.htm?abc=1';		
		else
			p_url = '/page-'+fid+'-'+t_p+'.htm?abc=1';
	 }else{
		if (tag == 'hot'){
			p_url = '/nhot-'+p_url;
			p_url = p_url.replace(/.htm/ig,"-1.htm");
		}else
			p_url = '/htm/'+p_url;
	}
	 var dm=get_para('dm');
	 if (dm != '') p_url = p_url+"&dm="+dm
	 var popup=get_para('popup');
	 if (popup != '') 
		p_url = p_url+'&popup=yes';
     self.location.href=p_url;
  }
  
var nowDialogId = null,dialog_w,dialog_h;
function showWin(id, w, h){
	if(nowDialogId)
		closeWin();

	nowDialogId = id;
	dialog_w = w || $("#"+nowDialogId).width();
	dialog_h = h || $("#"+nowDialogId).height();
	var height = $(document).height();
	var w1 = $(window).width();
	var h1 = $(window).height();
	var sh = $(window).scrollTop();
	var str = '<div style="position:absolute;left:0;top:0;background:#000;width:100%; height:'+height+'px; z-index:9999;" id="div_dialog">';
	$("body").append(str);
	$("#div_dialog").css("opacity","0.2");
	$("#"+nowDialogId).show().css({"top":parseInt((h1-dialog_h)/2+sh)+"px","left":parseInt((w1-dialog_w)/2),"zIndex":"99999","position":"absolute"});
	$(window).bind("resize", change_dialog);
	$(window).bind("scroll", change_dialog);
	return false;
}

function closeWin(){
	$("#div_dialog").remove();
	$("#"+nowDialogId).hide();
	$(window).unbind("resize",change_dialog);
	$(window).unbind("scroll",change_dialog);
	nowDialogId = null;
	return false;
}
function change_dialog(){
	var height = $(document).height();
	var w1 = $(window).width();
	var h1 = $(window).height();
	var sh = $(window).scrollTop();
	if(height != $('#div_dialog').height())
		$('#div_dialog').height(height);
	$("#"+nowDialogId).css({"top":parseInt((h1-dialog_h)/2+sh)+"px","left":parseInt((w1-dialog_w)/2),"zIndex":"99999","position":"absolute"});
	return false;
}  

function showWin2(obj,id, w, h){
	if(nowDialogId)
		closeWin2();
	last_div = obj;
	nowDialogId = id;
	dialog_w = w || $("#"+nowDialogId).width();
	dialog_h = h || $("#"+nowDialogId).height();
	var height = $(document).height();
	var w1 = $(window).width();
	var h1 = $(window).height();
	var sh = $(window).scrollTop();
	//var po_h = $(obj).offset().top+30;
	var po_h = $(obj).offset().top-235-50;	
	if (po_h < 0) po_h = 4;
	var str = '<div style="position:absolute;left:0;top:0;background:#000;width:100%; height:'+height+'px; z-index:9999;" id="div_dialog">';
	$("body").append(str);
	$("#div_dialog").css("opacity","0.2");
	$("#"+nowDialogId).show().css({"top":parseInt(po_h)+"px","left":parseInt((w1-dialog_w)/2),"zIndex":"99999","position":"absolute"});	
	//$("#"+nowDialogId).show().css({"top":parseInt((h1-dialog_h)/2+sh)+"px","left":parseInt((w1-dialog_w)/2),"zIndex":"99999","position":"absolute"});
	//$(window).bind("resize", change_dialog);
	//$(window).bind("scroll", change_dialog);
	return false;
}
function closeWin2(){
	$("#div_dialog").remove();
	$("#"+nowDialogId).hide();
	nowDialogId = null;
	return false;
}
//点赞
function send_flower(fid,cid,flag){
	$.ajax({  
		url:'//cdn.comment.4399pk.com/flower_new.php?fid='+fid+'&cid='+cid+'&flag='+flag+'&t='+Math.random(),
		dataType:"jsonp",
		callbackParameter:"callback",
		timeout:2000,
		success:function(result){
			if (result['status'] == 'success'){
				var tmps = document.getElementById('tag_'+cid).innerHTML;
				tmps =tmps.replace(/\[/i,"");
				tmps =tmps.replace(/\]/i,"");
				var tmpi = parseInt(tmps)+1;
				document.getElementById('tag_'+cid).innerHTML = '['+tmpi+']';
			}
			alert(result['msg']);		
		},  
		error:function(XMLHttpRequest, textStatus, errorThrown) {  
			alert("点赞失败，可能是网络不畅。");

		},  
		complete: function(XMLHttpRequest, textStatus) {}
	}); 
}

//向上或向下查看回复
function reply_chgpage(fid,cid,tag){
	var cur_pp = parseInt(document.getElementById('cur_p_'+cid).innerHTML);
	if (tag == 'pre'){
		if (cur_pp <= 1){
			alert("已经是第一页了");
			return false;
		}
		cur_pp--;
	}else
		cur_pp++;
	show_page(fid,cid,cur_pp);
	return false;
}
//跳到指定页
function reply_gopage(fid,cid){
	var cur_pp = parseInt(document.getElementById('goto_'+cid).value);
	show_page(fid,cid,cur_pp);	
	return false;	
}
//显示每页的用户回复
function show_page(fid,cid,p){
	$.post('//cdn.comment.4399pk.com/user_reply.php',{'fid':fid,'cid':cid,'p':p,t:Math.random()},function(result){
		if (result['status'] == 'success'){
			document.getElementById('cur_p_'+cid).innerHTML = result['cur_page'];
			document.getElementById('reply_'+cid).innerHTML = result['data'];
		}else
			show_msg('请求数据出错，请检查网络状况！');
	},'json');
	return false;
}

function show_reply_dialog(obj,fid,cid,tag){
	if(navigator.userAgent.indexOf('AppleWebKit') > -1){
		var receiver = document.getElementById('reply_iframe').contentWindow;
		receiver.postMessage(fid+"|"+cid+"|"+tag, location.protocol+"//"+_GA_("dm"));
		return false;
	}
	document.getElementById('reply_dialog').innerHTML = '<div id="my_reply" class="pop_n" style="left:0%; top:50px;">'+
		'<div class="pt"><span>我要回复</span><a href="#" onclick=\'closeWin2();return false;\'></a></div>'+
		'<div class="pc">'+
		'<div style="width:720px; background:#f5f8fd; overflow:hidden;">'+
		'<center>'+
		'<iframe id="my_pl_box" name="my_pl_box" style=" margin-left:-8px;_margin-left:-2px;" width="102%" scrolling="no" height="268" frameborder="0" marginheight=0 marginwidth=0 src="//www.4399.com/comment/reply_form_new.htm?fid='+fid+'&cid='+cid+'&tag='+tag+'"></iframe>'+
		'</center>'+
		'</div>'+
		'</div>'+
		'<div class="pb"></div>'+
		'</div>';
	showWin2(obj,'my_reply');
	return false;
}
