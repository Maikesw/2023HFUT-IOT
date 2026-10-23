package com.ahaxt.competition.utils;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.SignatureAlgorithm;

import java.util.Date;
import java.util.Map;

/**
 * @author hongzhangming
 */
public class JwtUtil {
    private static final String SECRET = "hongzhangming@foxmail.com";
    private static final String PREFIX = "token - ";

    public static String encode(Map<String, Object> body) {
        body.put("loginTime", new Date());
        return PREFIX + Jwts.builder()
                .setSubject("token body")
                .setClaims(body)
                .signWith(SignatureAlgorithm.HS512, SECRET)
                .compact();
    }

    public static Map<String, Object> deEncode(String token) {
        return Jwts.parser()
                .setSigningKey(SECRET)
                .parseClaimsJws(token.replace(PREFIX, ""))
                .getBody();
    }

}
