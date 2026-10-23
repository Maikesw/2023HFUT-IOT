package com.ahaxt.competition.config;

import org.hibernate.boot.model.naming.Identifier;
import org.hibernate.boot.model.naming.PhysicalNamingStrategy;
import org.hibernate.engine.jdbc.env.spi.JdbcEnvironment;
import org.springframework.util.StringUtils;

import java.util.Locale;

/**
 * @author hongzhangming
 */
public class JpaNamingStrategy implements PhysicalNamingStrategy {

    @Override
    public Identifier toPhysicalCatalogName(Identifier name, JdbcEnvironment jdbcEnvironment) {
        return toLowerCase(name, jdbcEnvironment);
    }

    @Override
    public Identifier toPhysicalSchemaName(Identifier name, JdbcEnvironment jdbcEnvironment) {
        return toLowerCase(name, jdbcEnvironment);
    }

    @Override
    public Identifier toPhysicalSequenceName(Identifier name, JdbcEnvironment jdbcEnvironment) {
        return toLowerCase(name, jdbcEnvironment);
    }

    //修改表名。首字母保持大写不变，后面小写变大写，大写加下划线
    @Override
    public Identifier toPhysicalTableName(Identifier name, JdbcEnvironment jdbcEnvironment) {
        return OurUnIdentifier(name,jdbcEnvironment);
        // return unChange(name, jdbcEnvironment);
    }

    //修改列/字段名。小写变大写，大写加下划线
    @Override
    public Identifier toPhysicalColumnName(Identifier name, JdbcEnvironment jdbcEnvironment) {
        return OurIdentifier(name,jdbcEnvironment);
        // return unChange(name, jdbcEnvironment);
    }

    private Identifier OurIdentifier(Identifier name, JdbcEnvironment jdbcEnvironment) {
        if (StringUtils.isEmpty(name)) {
            return null;
        }
        String s = name.getText().substring(1);
        for (char c='A'; c<='Z';c++) {
            s = s.replaceAll(String.valueOf(c),"_"+c);
        }
        s = (name.getText().substring(0,1) + s).toUpperCase(Locale.ROOT);
        return new Identifier(s,name.isQuoted());

    }

    private Identifier OurUnIdentifier(Identifier name, JdbcEnvironment jdbcEnvironment) {
        if (StringUtils.isEmpty(name)) {
            return null;
        }
        String s = name.getText().substring(1);
        for (char c='A'; c<='Z';c++) {
            s = s.replaceAll(String.valueOf(c),"_"+c);
        }
        s = (name.getText().substring(0,1) + s).toLowerCase(Locale.ROOT);
        return new Identifier(s,name.isQuoted());

    }

    private Identifier unChange(Identifier name, JdbcEnvironment jdbcEnvironment) {
        if (StringUtils.isEmpty(name)) {
            return null;
        }
        return new Identifier(name.getText(), name.isQuoted());
    }

    private Identifier toLowerCase(Identifier name, JdbcEnvironment jdbcEnvironment) {
        if (StringUtils.isEmpty(name)) {
            return null;
        }
        return new Identifier(name.getText().toLowerCase(Locale.ROOT), name.isQuoted());
    }
    private Identifier toUpperCase(Identifier name, JdbcEnvironment jdbcEnvironment) {
        if (StringUtils.isEmpty(name)) {
            return null;
        }
        return new Identifier(name.getText().toUpperCase(Locale.ROOT), name.isQuoted());
    }

}
