package com.ahaxt.competition.utils;



import com.ahaxt.competition.config.FreemarkerOutputFormat;
import freemarker.template.Configuration;
import freemarker.template.Template;

import java.io.*;
import java.util.Map;

/**
 * @author gaowen
 * @Desc：word操作工具类
 */
public class WorldUtil {

    /**
     * @Desc：生成word文件
     * @param dataMap
     *            word中需要展示的动态数据，用map集合来保存
     * @param templateName
     *            word模板名称，例如：test.ftl
     *
     */
    public static void createWorld(Map dataMap, String templateName, String fileName) {
        try {

            //1.创建配置类
            Configuration configuration = new Configuration();
            //2.设置模板所在的目录
            configuration.setClassForTemplateLoading(WorldUtil.class, "/");
            //3.设置字符集
            configuration.setDefaultEncoding("utf-8");
            // 自定义特殊字符转换规则
            configuration.setOutputFormat(  FreemarkerOutputFormat.INSTANCE);
            //4.加载模板;
            Template template = configuration.getTemplate(templateName);
            // 输出文件
              File outFile = new File( fileName);
            // 如果输出目标文件夹不存在，则创建
              if (!outFile.getParentFile().exists()) {
                outFile.getParentFile().mkdirs();
            }
            // 将模板和数据模型合并生成文件
            Writer out = new BufferedWriter(new OutputStreamWriter(new FileOutputStream(outFile), "utf-8"));

            // 生成文件
            template.process(dataMap, out);

            // 关闭流
            out.flush();
            out.close();

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}