package com.ahaxt.competition.utils;

import com.lowagie.text.pdf.BaseFont;
import freemarker.template.Template;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.web.servlet.view.freemarker.FreeMarkerConfigurer;
import org.xhtmlrenderer.pdf.ITextFontResolver;
import org.xhtmlrenderer.pdf.ITextRenderer;

import java.io.*;
import java.util.ArrayList;
import java.util.List;


/**
 * <p>
 * 用于生成获奖证书
 * </p>
 * @author : mazhaoxing
 * @date : 2022/11/23 14:19
 */
public class PdfUtils {
    private PdfUtils() {
    }

    private static final Logger LOGGER = LoggerFactory.getLogger(PdfUtils.class);

    /**
     * 按模板和参数生成html字符串
     * @param configurer
     * @param templateName freemarker模板名称
     * @param variables    freemarker模板参数
     * @return String
     * <p>
     * 解决打包后Windows下调用出现空指针问题
     */
    private static String generateDocString(FreeMarkerConfigurer configurer, String templateName, Object variables){
        StringWriter writer = new StringWriter();
        try {
            Template template = configurer.getConfiguration().getTemplate(templateName);
            template.process(variables, writer);
        }catch (Exception e){
            LOGGER.error(e.getMessage(),e);
        }
        writer.flush();
        return writer.toString();
    }}
