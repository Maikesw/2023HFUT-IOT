package com.ahaxt.competition.utils;

import org.slf4j.Logger;

import java.util.stream.Collectors;
import java.util.stream.IntStream;

/**
 * @author hongzhangming
 */
public class LogUtil {

    /**
     * 筛选无效日志，仅打印前 row 行
     */
    public static void error(Logger logger, Exception e) {
        error(logger, e, 99);
    }

    /**
     * 筛选无效日志，仅打印前 row 行
     * @param logger
     * @param e
     * @param row
     */
    public static void error(Logger logger, Exception e, int row) {
        logger.error(IntStream.range(0, Math.min(e.getStackTrace().length, row)).mapToObj(i -> "\n\t" + e.getStackTrace()[i]).collect(Collectors.joining("", "\n" + e.toString(), "\n\t...")));
    }

}
