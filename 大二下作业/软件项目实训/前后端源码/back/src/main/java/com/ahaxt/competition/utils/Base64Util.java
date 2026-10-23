package com.ahaxt.competition.utils;

import com.ahaxt.competition.repository.db.SysOssFileDao;

import javax.annotation.Resource;
import java.io.UnsupportedEncodingException;
import java.nio.charset.StandardCharsets;
import java.util.Base64;

/**
 * @author hongzhangming
 */
public class Base64Util {


    public static Object encoder(byte[] textByte) {
        Base64.Encoder encoder = Base64.getEncoder();
        String encodedText = encoder.encodeToString(textByte);
        return encodedText;
    }

    public static String decoderToStr(String encodedText) throws UnsupportedEncodingException {
        byte[] textByte = Base64.getDecoder().decode(encodedText);
        String res =  new String(textByte, StandardCharsets.UTF_8);
//        textByte = res.getBytes("UTF-8");
        return res;
    }

    public static byte[] decoder(String encodedText) {
        byte[] textByte = Base64.getDecoder().decode(encodedText);
        return textByte;
    }

//    public static String getFirstImg(Integer Id){
//        String imgUrl = "";
//        sysOssFileDao.getByIdAndIsDeletedFalse(id);
//        Integer index = text.indexOf("https://127.0.0.1:9000/competition");
//        if(index>0){
//            imgUrl = text.substring(index,text.indexOf("\"",index));
//        }
//        return imgUrl;
//    }
}
