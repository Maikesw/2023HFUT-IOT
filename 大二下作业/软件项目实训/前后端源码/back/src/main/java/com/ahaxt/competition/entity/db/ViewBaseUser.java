package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.NameRemarkInfo;
import lombok.Data;

import javax.persistence.Entity;
import java.util.Date;


@Data
@Entity
public class ViewBaseUser extends NameRemarkInfo {
    private Boolean firstLogin;
    private String phone;
    private String account;
    private String password;
    private String sex;
    private String idCard;
    private Date birth;
    private String address;
    private String postalCode;
    private String JobId;
    private String email;
    //private Date registerTime;
    private String nickName;
    private Integer departmentId;
    private Integer schoolId;
    private String workId;
    private String themeColor;
    private String DepartmentName;
    private String JobName;
}
