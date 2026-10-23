package com.ahaxt.competition.utils;

import cn.hutool.core.io.FileUtil;
import com.aspose.words.Document;
import com.aspose.words.FontSettings;
import com.aspose.words.PdfSaveOptions;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import java.io.File;


/**
 * description:
 *
 * @author gaowen
 * @date 2020/04/23
 */

public class WordToPdf {
    private static Logger log = LoggerFactory.getLogger(WordToPdf.class);

    public static void worldToPdf(String docPath, String pdfPath) {
        File file = null;
        try {
            //doc路径
            Document document = new Document(docPath);
            //pdf路径
            File outputFile = new File(pdfPath);
            PdfSaveOptions options = new PdfSaveOptions();
            options.setExportCustomPropertiesAsMetadata(true);
            //操作文档保存
            document.save(outputFile.getAbsolutePath(), com.aspose.words.SaveFormat.PDF);

            String os = System.getProperty("os.name");
            //判断当前操作系统是不是linux
            if(!os.toLowerCase().startsWith("win")){
                FontSettings.setFontsFolder("/usr/share/fonts/windows", false);
            }else{
                FontSettings.setFontsFolder("C:\\Windows\\Fonts", false);
            }
            //删除本地的doc文件
            file = new File(docPath);
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            FileUtil.del(file);
        }
    }
}


