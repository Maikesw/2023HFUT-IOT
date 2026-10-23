package com.ahaxt.competition.config.shiro;

import com.ahaxt.competition.base.BaseResponse;
import com.ahaxt.competition.utils.FastJsonUtil;
import org.apache.shiro.mgt.SecurityManager;
import org.apache.shiro.session.mgt.SessionManager;
import org.apache.shiro.spring.security.interceptor.AuthorizationAttributeSourceAdvisor;
import org.apache.shiro.spring.web.ShiroFilterFactoryBean;
import org.apache.shiro.web.filter.authc.UserFilter;
import org.apache.shiro.web.mgt.CookieRememberMeManager;
import org.apache.shiro.web.mgt.DefaultWebSecurityManager;
import org.apache.shiro.web.servlet.SimpleCookie;
import org.apache.shiro.web.session.mgt.DefaultWebSessionManager;
import org.springframework.aop.framework.autoproxy.DefaultAdvisorAutoProxyCreator;
import org.springframework.boot.autoconfigure.condition.ConditionalOnMissingBean;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import javax.servlet.Filter;
import javax.servlet.ServletRequest;
import javax.servlet.ServletResponse;
import javax.servlet.http.HttpServletResponse;
import java.io.IOException;
import java.util.HashMap;
import java.util.LinkedHashMap;
import java.util.Map;

/**
 * @author hongzhangming
 */
@Configuration
public class ShiroConfig {

    /**
     * defaultAdvisorAutoProxyCreator
     * @return
     */
    @Bean
    @ConditionalOnMissingBean
    public DefaultAdvisorAutoProxyCreator defaultAdvisorAutoProxyCreator() {
        DefaultAdvisorAutoProxyCreator defaultAAP = new DefaultAdvisorAutoProxyCreator();
        defaultAAP.setProxyTargetClass(true);
        defaultAAP.setUsePrefix(true);
        return defaultAAP;
    }

    /**
     * authorizationAttributeSourceAdvisor
     * @param securityManager
     * @return
     */
    @Bean
    public AuthorizationAttributeSourceAdvisor authorizationAttributeSourceAdvisor(SecurityManager securityManager) {
        AuthorizationAttributeSourceAdvisor authorizationAttributeSourceAdvisor = new AuthorizationAttributeSourceAdvisor();
        authorizationAttributeSourceAdvisor.setSecurityManager(securityManager);
        return authorizationAttributeSourceAdvisor;
    }

    /**
     * securityManager
     * @return
     */
    @Bean
    public SecurityManager securityManager() {
        DefaultWebSecurityManager securityManager = new DefaultWebSecurityManager();
        securityManager.setRealm(customRealm());
        securityManager.setRememberMeManager(cookieRememberMeManager());
        securityManager.setSessionManager(sessionManager());
        return securityManager;
    }

    /**
     * 将自己的验证方式加入容器
     * @return
     */
    @Bean
    public CustomRealm customRealm() {
        return new CustomRealm();
    }

    /**
     * cookieRememberMeManager
     * @return
     */
    @Bean
    public CookieRememberMeManager cookieRememberMeManager() {
        CookieRememberMeManager cookieRememberMeManager = new CookieRememberMeManager();

        // 设置cookie名称，对应login.html页面的<input type="checkbox" name="rememberMe"/>
        SimpleCookie cookie = new SimpleCookie("rememberMe");
        // 设置cookie的过期时间，单位为秒，这里为一天
        cookie.setMaxAge(86400);
        cookieRememberMeManager.setCookie(cookie);
        cookieRememberMeManager.setCipherKey("Hzm:QQ1121508290".getBytes());
        return cookieRememberMeManager;
    }

    /**
     * sessionManager
     * @return
     */
    @Bean
    public SessionManager sessionManager() {
        DefaultWebSessionManager sessionManager = new DefaultWebSessionManager();
        sessionManager.setGlobalSessionTimeout(2 * 60 * 60 * 1000L);
        return sessionManager;
    }

    /**
     * Filter工厂，设置对应的过滤条件和跳转条件
     * <p>
     * anon: 无需认证即可访问
     * authc: 需要认证才可访问
     * user: 点击“记住我”功能可访问
     * perms: 拥有权限才可以访问
     * role: 拥有某个角色权限才能访问
     * @param securityManager
     * @return
     */
    @Bean
    public ShiroFilterFactoryBean shiroFilterFactoryBean(SecurityManager securityManager) {
        ShiroFilterFactoryBean shiroFilterFactoryBean = new ShiroFilterFactoryBean();
        /**
         * setSecurityManager
         */
        shiroFilterFactoryBean.setSecurityManager(securityManager);
        /**
         * setFilters
         */
        Map<String, Filter> filters = new HashMap<>(8);
        //redirectToLogin
        filters.put("authc", new UserFilter() {
            @Override
            protected void redirectToLogin(ServletRequest request, ServletResponse response) throws IOException {
                HttpServletResponse httpServletResponse = ((ServletRequestAttributes) RequestContextHolder.getRequestAttributes()).getResponse();
                httpServletResponse.setStatus(BaseResponse.unAuthorization.getHttpStatus());
                response.setContentType("application/json; charset=utf-8");
                response.getWriter().write(FastJsonUtil.toString(BaseResponse.unAuthorization));
            }
        });
        shiroFilterFactoryBean.setFilters(filters);
        /**
         * setFilterChainDefinitionMap
         */
        Map<String, String> map = new LinkedHashMap<>(17);
//        map.put("/sign/info", "authc");
//        map.put("/sign/userList", "authc");
        map.put("/sign/verCode/**","anon");
        map.put("/sign/checkVerCode","anon");
        map.put("/sign/login", "anon");
        map.put("/sign/editPassword", "anon");
        map.put("/sign/**","anon");

        map.put("/website/column","anon");
        map.put("/website/homePage","anon");
        map.put("/website/columnContent","anon");
        map.put("/website/readNum","anon");
        map.put("/website/search","anon");

        map.put("/channel/saveContent","anon");

        //sockJs
        map.put("/sockJs/**", "anon");
        map.put("/socketJS/**", "anon");
        //swagger
        map.put("/swagger-ui.html", "anon");
        map.put("/webjars/**", "anon");
        map.put("/v2/**", "anon");
        map.put("/swagger-resources/**", "anon");
        map.put("/common/oss/**", "anon");
        //默认
        map.put("/**", "authc");

        shiroFilterFactoryBean.setFilterChainDefinitionMap(map);
        return shiroFilterFactoryBean;
    }


}