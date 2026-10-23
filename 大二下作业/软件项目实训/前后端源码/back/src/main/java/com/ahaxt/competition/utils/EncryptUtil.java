package com.ahaxt.competition.utils;
import javax.annotation.Resource;
import javax.crypto.Cipher;
import javax.crypto.KeyGenerator;
import javax.crypto.spec.SecretKeySpec;

import com.alibaba.fastjson.JSONObject;
import com.google.gson.JsonObject;
import org.springframework.stereotype.Component;
import java.util.Base64;

import java.util.*;

@Component
public class EncryptUtil {

    @Resource
    protected RedisUtil redis;


    private final String KEY = "abcdefgabcdefg12";
    private final String ALGORITHMSTR = "AES/ECB/PKCS5Padding";

    public static List<String> allKeys = new ArrayList<>();
    public static List<Boolean> haveKeys = new ArrayList<>();
    public static List<Date> KeysDate = new ArrayList<>();

    private Integer getSuitablePos() {
        Integer ret = -1;
        boolean flag = false;
        int i=0;
        Date nowDate = new Date();
        while (i<haveKeys.size() && !flag) {
            Date keyDate = KeysDate.get(i);
            long ll = nowDate.getTime() - keyDate.getTime();
            if (ll > 300000 || !haveKeys.get(i)) {
                ret  = i;
                flag = true;
            } else {
                i++;
            }
        }
        return ret;
    }


    public JSONObject setAllKeys(String key) {
        Integer pos = getSuitablePos();
        if (pos == -1) {
            allKeys.add(key);
            haveKeys.add(true);
            KeysDate.add(new Date());
            JSONObject val = new JSONObject();
            val.put("key", key);
            val.put("value", allKeys.size()-1);
            return val;
        } else {
            haveKeys.set(pos, true);
            KeysDate.set(pos, new Date());
            JSONObject val = new JSONObject();
            val.put("key", allKeys.get(pos));
            val.put("value", pos);
            return val;
        }
    }


    public String base64Encode(byte[] bytes){
        return org.apache.commons.codec.binary.Base64.encodeBase64String(bytes);
    }
    public byte[] base64Decode(String base64Code) throws Exception{
        return Base64.getDecoder().decode(base64Code);
    }
    public byte[] aesEncryptToBytes(String content, String encryptKey) throws Exception {
        KeyGenerator kgen = KeyGenerator.getInstance("AES");
        kgen.init(128);
        Cipher cipher = Cipher.getInstance(ALGORITHMSTR);
        cipher.init(Cipher.ENCRYPT_MODE, new SecretKeySpec(encryptKey.getBytes(), "AES"));

        return cipher.doFinal(content.getBytes("utf-8"));
    }
    public String aesEncrypt(String content, String encryptKey) throws Exception {
        return base64Encode(aesEncryptToBytes(content, encryptKey));
    }
    public String aesDecryptByBytes(byte[] encryptBytes, String decryptKey) throws Exception {
        KeyGenerator kgen = KeyGenerator.getInstance("AES");
        kgen.init(128);

        Cipher cipher = Cipher.getInstance(ALGORITHMSTR);
        cipher.init(Cipher.DECRYPT_MODE, new SecretKeySpec(decryptKey.getBytes(), "AES"));
        byte[] decryptBytes = cipher.doFinal(encryptBytes);

        return new String(decryptBytes);
    }
    public String aesDecrypt(String encryptStr, String decryptKey) throws Exception {
        return aesDecryptByBytes(base64Decode(encryptStr), decryptKey);
    }

    public String getKeyWord(String keyWord) {
        String decryptData = "";
        int value = Integer.parseInt(keyWord.substring(keyWord.length()-2,keyWord.length()));
        Date nowDate = new Date();
        Date keyDate = KeysDate.get(value);
        long ll = nowDate.getTime() - keyDate.getTime();
        if (haveKeys.get(value) && (ll<300000)) {
            try {
                String key = "";
                if (value < 10) {
                    key = allKeys.get(value) + "0" + value + (new StringBuilder(allKeys.get(value)).reverse().toString());
                } else {
                    key = allKeys.get(value) + value + (new StringBuilder(allKeys.get(value)).reverse().toString());
                }
                decryptData = aesDecrypt(keyWord.substring(0, keyWord.length() - 2), key);
                //用后即焚
                haveKeys.set(value, false);
            } catch (Exception ex) {

            }
        }
        return decryptData;
    }


}
