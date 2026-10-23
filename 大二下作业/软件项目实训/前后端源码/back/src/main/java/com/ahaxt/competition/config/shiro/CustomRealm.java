package com.ahaxt.competition.config.shiro;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.repository.db.BaseUserDao;
import com.ahaxt.competition.utils.EncodeUtil;
import com.ahaxt.competition.utils.RedisUtil;
import com.alibaba.fastjson.JSONObject;
import org.apache.shiro.authc.*;
import org.apache.shiro.authc.credential.CredentialsMatcher;
import org.apache.shiro.authc.credential.SimpleCredentialsMatcher;
import org.apache.shiro.authz.AuthorizationInfo;
import org.apache.shiro.authz.SimpleAuthorizationInfo;
import org.apache.shiro.realm.AuthorizingRealm;
import org.apache.shiro.subject.PrincipalCollection;

import javax.annotation.Resource;
import java.util.stream.Collectors;

import static com.ahaxt.competition.base.Constant.APPLICATION_NAME;


/**
 * @author hongzhangming
 */
@SuppressWarnings("unchecked")
public class CustomRealm extends AuthorizingRealm {
    @Resource
    private Base base;
    @Resource
    private RedisUtil redis;

    @Resource
    private BaseUserDao tblUserInfoDao;

    /**
     * 获取身份验证信息
     * @param authenticationToken
     * @return
     * @throws AuthenticationException
     */
    @Override
    protected AuthenticationInfo doGetAuthenticationInfo(AuthenticationToken authenticationToken) throws AuthenticationException {
        UsernamePasswordToken usernamePasswordToken = (UsernamePasswordToken) authenticationToken;
        return new SimpleAuthenticationInfo(usernamePasswordToken.getUsername(), tblUserInfoDao.getByIdAndIsDeletedFalse(Integer.valueOf(usernamePasswordToken.getUsername())).getPassword(), getName());
    }

    /**
     * 设置凭据匹配器
     * @param
     */
    @Override
    public void setCredentialsMatcher(CredentialsMatcher credentialsMatcher) {
        super.setCredentialsMatcher(new SimpleCredentialsMatcher() {
            @Override
            public boolean doCredentialsMatch(AuthenticationToken authenticationToken, AuthenticationInfo authenticationInfo) {
                UsernamePasswordToken usernamePasswordToken = (UsernamePasswordToken) authenticationToken;
                return equals(EncodeUtil.pwdShiro(new String(usernamePasswordToken.getPassword()), usernamePasswordToken.getUsername()), getCredentials(authenticationInfo));
            }
        });
    }

    /**
     * 获取授权信息
     * @param principalCollection
     * @return
     */
    @Override
    protected AuthorizationInfo doGetAuthorizationInfo(PrincipalCollection principalCollection) {
        String key = APPLICATION_NAME + ":authorizationInfo:" + base.getLoginUserId();
        if (!redis.hasKey(key)) {
            SimpleAuthorizationInfo simpleAuthorizationInfo = new SimpleAuthorizationInfo();
            JSONObject json = (JSONObject) base.getLoginUser();
            simpleAuthorizationInfo.addRoles(json.getJSONArray("roles").stream().map(String::valueOf).collect(Collectors.toSet()));
            simpleAuthorizationInfo.addStringPermissions(json.getJSONArray("permissions").stream().map(String::valueOf).collect(Collectors.toSet()));
            redis.set(key, simpleAuthorizationInfo, 0x12c);
        }
        return (AuthorizationInfo) redis.get(key);
    }
}
