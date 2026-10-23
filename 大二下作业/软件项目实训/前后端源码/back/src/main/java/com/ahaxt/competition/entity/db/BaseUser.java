package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.NameRemarkInfo;
import lombok.Getter;
import lombok.Setter;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Transient;
import java.util.Date;
import java.util.List;

@Getter
@Setter
@Entity
public class BaseUser extends NameRemarkInfo {
    @Column(columnDefinition = "bit(1) default b'1' comment'首次登陆'")
    private Boolean firstLogin;
    @Column(columnDefinition = "varchar(50) CHARACTER SET utf8 COLLATE utf8_bin COMMENT '手机号码'")
    private String phone;
    @Column(columnDefinition = "varchar(50) CHARACTER SET utf8 COLLATE utf8_bin COMMENT '账户'")
    private String account;
    @Column(columnDefinition = "varchar(50) CHARACTER SET utf8 COLLATE utf8_bin")
    private String password;
    @Column(columnDefinition = "char(2) comment '性别'")
    private String sex;
    @Column(columnDefinition = "varchar(50) comment '身份证号'")
    private String idCard;
    @Column(columnDefinition = "datetime comment'出生日期'")
    private Date birth;
    @Column(columnDefinition = "varchar(50) comment'地址'")
    private String address;
    @Column(columnDefinition = "varchar(50) comment'邮政编码'")
    private String postalCode;
    @Column(columnDefinition = "varchar(50) comment'电子邮件'")
    private String email;
//    @Column(columnDefinition = "datetime comment '注册时间'")
//    private Date registerTime;
    @Column(columnDefinition = "varchar(50) CHARACTER SET utf8 COLLATE utf8_bin COMMENT '昵称'")
    private String nickName;
    @Column(columnDefinition = "integer unsigned comment '部门ID'")
    private Integer departmentId;
    @Column(columnDefinition = "integer unsigned comment '身份ID'")
    private Integer jobId;
    @Column(columnDefinition = "varchar(50) comment '工号'")
    private String workId;
    @Column(columnDefinition = "varchar(50) CHARACTER SET utf8 COLLATE utf8_bin COMMENT '主题色'")
    private String themeColor;
    @Transient
    private String job;
    @Transient
    private String role;
    @Transient
    private String departmentName;
    @Transient
    private List<Integer> userRoleRelIds;
    @Transient
    private List<Integer> roleIds;
    @Transient
    private Integer ossFileId;
    @Transient
    private String headImage;
}
