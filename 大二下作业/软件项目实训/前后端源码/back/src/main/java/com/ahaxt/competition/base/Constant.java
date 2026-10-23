package com.ahaxt.competition.base;

import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Configuration;

/**
 * @author hongzhangming
 */
@Configuration
public class Constant {
    /**
     * orAndNon
     */
    public static final String OR = "||";
    public static final String AND = "&&";
    public static final String NOT = "!";
    public static final String LT = "<";
    public static final String GT = ">";
    public static final String EQ = "=";
    public static final String LE = "<=";
    public static final String GE = ">=";
    public static final String NE = "!=";
    public static final String LIKE = "≈";
    public static final String IN = "()";
    public static final String NOT_IN = "!()";
    public static final String RANGE = "<=>";
    /**
     * APPLICATION_NAME 项目名
     * FILE_SEPARATOR 自定义统一文本分隔符
     */
    public static String APPLICATION_NAME, FILE_SEPARATOR;

    @Value("${spring.application.name}")
    public void setApplicationName(String applicationName) {
        APPLICATION_NAME = applicationName;
        FILE_SEPARATOR = "@" + applicationName + "@";
    }

    /**
     * AXT-PUBLIC ossBucketName
     */
    public static String AXT_PUBLIC;

//    @Value("${oss.bucketName.public}")
//    public void setBucketName(String bucketName) {
//        AXT_PUBLIC = bucketName;
//    }
    /**
     * entityPackage
     * repositoryPackage
     */
    public static String entityPackage, repositoryPackage;

    @Value(value = "${entity.package}")
    public void setEntityPackage(String entPackage) {
        entityPackage = entPackage;
    }

    @Value(value = "${repository.package}")
    public void setRepositoryPackage(String repPackage) {
        repositoryPackage = repPackage;
    }

    /**
     * permission suffix
     */
    public static final String CREATE = ":c";
    public static final String RETRIEVE = ":r";
    public static final String UPDATE = ":u";
    public static final String DELETE = ":d";



    /**
     * Regular expression
     */
    public static final String
            REGEX_MOBILE = "^1[3-9]\\d{9}$",
            REGEX_EMAIL = "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$";


    public static final int  DEFAULT_PAGE = 1;
    public static final int DEFAULT_SIZE = 25;

    public static final String USER_INFO = "BaseUser"; //用户表
    public static final String DEPARTMENT_INFO = "BaseDepartment"; //部门信息表
    public static final String OSS_FILE_INFO = "SysOssFile"; //OSS表

    /**
     * oss文件类型
     */
    public static final int HEAD_IMAGE = 1;


    public static final class SPLIT_OPERATOR {
        public static final String VERTICALLINE = "|";
        public static final String COMMA =  ",";
        public static final String DOT = ".";
        public static final String AND = "&";
    }
    public static final class ROLE_TABLE {
        public static final int SUPER_ADMIN = 1;
        public static final int TEACHER = 2;
        public static final int STUDENT = 3;
        public static final int EXPERT = 4;
        public static final int CONTEST_ADMIN = 5;
        public static final int WEB_ADMIN = 6;
        public static final int UNIVERSITY_ADMIN = 7;


    }
    public static final class AUDIT_STATUS {
        public static final int SAVE = -1;
        public static final String SAVENAME = "保存未提交";
        public static final int SUBMIT = 0;
        public static final String SUBMITNAME = "提交待审核";
        public static final int PASS = 1;
        public static final String PASSNAME = "审核通过";
        public static final int NOTPASS = 2;
        public static final String NOTPASSNAME = "审核未通过";
        public static final int BACK = 3;
        public static final String BACKNAME = "审核退回";
    }

    public static final String BUCKET_NAME = "association";




}
