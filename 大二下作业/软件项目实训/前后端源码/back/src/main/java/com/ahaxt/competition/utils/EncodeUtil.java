package com.ahaxt.competition.utils;

import org.apache.shiro.crypto.hash.SimpleHash;
import org.springframework.util.DigestUtils;

import java.io.UnsupportedEncodingException;
import java.nio.charset.StandardCharsets;
import java.util.HashMap;
import java.util.Map;

/**
 * @author hongzhangming
 */
public class EncodeUtil {
    /**
     * shiro 加密方式
     * @param source password
     * @param salt   userId
     * @return
     */
    public static String pwdShiro(String source, Object salt) {
//        if (!isStrongPwd(source)) {
//            throw BaseResponse.weakPassword.error();
//        }
        return new SimpleHash("md5", source, salt.toString().getBytes(), 3).toHex();
    }


    /**
     * md5 加密
     * @param param
     * @param salt
     * @return
     * @throws UnsupportedEncodingException
     */
    private static String encode(String param, Object salt) throws UnsupportedEncodingException {
        return DigestUtils.md5DigestAsHex((param + salt).getBytes(StandardCharsets.UTF_8));
    }

    public static boolean isStrongPwd(String password) {
        Map<String, String> map = new HashMap<>(4);
        for (int i = 0; i < password.length(); i++) {
            int A = password.charAt(i);
            if (A >= 48 && A <= 57) {
                map.put("数字", "数字");
            } else if (A >= 65 && A <= 90) {
                map.put("大写", "大写");
            } else if (A >= 97 && A <= 122) {
                map.put("小写", "小写");
            } else {
                map.put("特殊", "特殊");
            }
        }
        return map.keySet().size() >= 4 && password.length() >= 8;
    }
}
