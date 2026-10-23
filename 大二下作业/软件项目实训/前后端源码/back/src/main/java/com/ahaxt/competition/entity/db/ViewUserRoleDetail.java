package com.ahaxt.competition.entity.db;

import com.ahaxt.competition.entity.base.BaseInfo;
import lombok.Data;

import javax.persistence.Entity;

@Data
@Entity
public class ViewUserRoleDetail extends BaseInfo {
    private Integer roleId;
    private Integer userId;
    private Integer isAudit;
    private String roleName;
    private String roleCode;
    private String userName;
    private String userPhone;
    private String departmentName;
}
