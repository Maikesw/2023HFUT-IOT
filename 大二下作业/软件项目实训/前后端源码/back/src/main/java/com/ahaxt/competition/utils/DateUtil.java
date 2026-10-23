package com.ahaxt.competition.utils;

import org.springframework.stereotype.Component;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.GregorianCalendar;

/**
 * 时间工具类
 * @author hongzhangming
 */
@Component
public class DateUtil {

    public static Date toDate(String dateStr, String pattern) throws ParseException {
        return new SimpleDateFormat(pattern).parse(dateStr);
    }

    public static String toString(long timeMillis, String pattern) {
        return toString(new Date(timeMillis), pattern);
    }

    public static String toString(Date date, String pattern) {
        return new SimpleDateFormat(pattern).format(new Date());
    }

    /**
     * 获取今年是哪一年
     * @return
     */
    public static Integer year(Date date) {
        GregorianCalendar gc = (GregorianCalendar) Calendar.getInstance();
        gc.setTime(date);
        return Integer.valueOf(gc.get(1));
    }

    /**
     * 将时间转化为string格式 如：yyyy-MM-dd HH:mm:ss
     * @param date
     * @return
     */
    public static String format(Date date, String format) {
        DateFormat formatter = new SimpleDateFormat(format);
        return formatter.format(date);
    }

    /**
     * 耗时
     * 毫秒数 => 天,时:分:秒.毫秒
     */
    public static String formatDate(long timeMillis) {
        long day = timeMillis / (24 * 60 * 60 * 1000);
        long hour = (timeMillis / (60 * 60 * 1000) - day * 24);
        long min = ((timeMillis / (60 * 1000)) - day * 24 * 60 - hour * 60);
        long ss = (timeMillis / 1000 - day * 24 * 60 * 60 - hour * 60 * 60 - min * 60);
        long sss = (timeMillis - day * 24 * 60 * 60 * 1000 - hour * 60 * 60 * 1000 - min * 60 * 1000 - ss * 1000);
        return (day > 0 ? day + "天," : "") + hour + ":" + min + ":" + ss + "." + sss;
    }

}
