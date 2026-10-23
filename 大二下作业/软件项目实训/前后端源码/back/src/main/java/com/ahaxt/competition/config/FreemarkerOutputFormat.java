package com.ahaxt.competition.config;


import freemarker.core.CommonMarkupOutputFormat;
import freemarker.core.TemplateXMLOutputModel;
import freemarker.template.utility.StringUtil;

import java.io.IOException;
import java.io.Writer;

/**
 * @author qc
 * @date 2021年05月21日 11:51
 */

public final class FreemarkerOutputFormat extends CommonMarkupOutputFormat<TemplateXMLOutputModel> {

    public static final FreemarkerOutputFormat INSTANCE = new FreemarkerOutputFormat();

    private FreemarkerOutputFormat() {

    }



    @Override
    public String getName() {
        return "XML";
    }

    @Override
    public String getMimeType() {
        return "application/xml";
    }

    @Override
    public void output(String textToEsc, Writer out) throws IOException {
        XMLOrHTMLEnc(textToEsc, out);
    }

    @Override
    public String escapePlainText(String plainTextContent) {
        return StringUtil.XMLEnc(plainTextContent);
    }

    @Override
    public boolean isLegacyBuiltInBypassed(String builtInName) {
        return builtInName.equals("xml");
    }

    @Override
    protected TemplateXMLOutputModel newTemplateMarkupOutputModel(String plainTextContent, String markupContent) {
        return null;
    }

    private void XMLOrHTMLEnc(String s,  Writer out) throws IOException{
        // 替换&
        s = s.replace("&",  "&amp;");
        // 替换<
        s = s.replace("<",  "&lt;");
        // 替换>
        s = s.replace(">", "&gt;");
        // 替换单引号'
        s = s.replace("'", "&apos;");
        // 替换双引号"
        s = s.replace("\"", "&quot;");
        s = s.replace("\r\n", "<w:br/>");
        s = s.replace("\n", "<w:br/>");
        s = s.replace(" ", "&#160;");
        s = s.replace("\t", "&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;");

        char[] aChar = s.toCharArray();
        out.write(aChar);
    }

}
