package com.ahaxt.competition.utils;


import com.ahaxt.competition.base.BaseResponse;
import org.springframework.util.StringUtils;
import org.springframework.web.multipart.MultipartFile;

import java.io.*;


/**
 * @author hongzhangming
 */
public class FileUtil {
    private static volatile File searchKeysFile = null;

    public static void main(String[] args) {
    }

    /*  *//** 文件写入新数据 *//*
    public synchronized static boolean appendData(String newStr)  {
        boolean response = false;
        FileWriter fileWriter;
        try {
            File file = new File(Base.LOG_FILE);
            if(!file.exists() || !file.isFile()){
                file = createFile(Base.LOG_FILE, Base.LOG_FOLDER);
                fileWriter = new FileWriter(Base.LOG_FILE,true);
                fileWriter.append(1 + "\r\n");
                fileWriter.close();
            }
            if(getFileSize(file)> Base.LOG_SIZE){
                String temp;
                FileInputStream fis = new FileInputStream(file);
                InputStreamReader isr = new InputStreamReader(fis);
                BufferedReader br = new BufferedReader(isr);
                boolean flag = (temp = br.readLine()) != null;
                fis.close();
                isr.close();
                br.close();
                if (flag) {
                    renameTo(Base.LOG_FILE, Base.LOG_FOLDER+File.separator+temp+".log");
                    createFile(Base.LOG_FILE, Base.LOG_FOLDER);
                    fileWriter = new FileWriter(Base.LOG_FILE,true);
                    fileWriter.append((Integer.valueOf(temp)+1) + "\r\n");
                    fileWriter.close();
                }
            }
            fileWriter = new FileWriter(Base.LOG_FILE,true);
            fileWriter.append(newStr.replaceAll("(\r\n|\r|\n|\n\r)", "<br>") + "\r\n");
            fileWriter.close();
            response = true;
        }catch (Exception e){
            e.printStackTrace();
        }finally {
            return response;
        }
    }*/

    /**
     * rename
     */
    private static void renameTo(String oldFile, String newFile) {
        new File(oldFile).renameTo(new File(newFile));
    }

    /**
     * 获取文件长度:https://www.cnblogs.com/hellowhy/p/7238570.html
     */
    public static long getFileSize(File file) {
        if (file.exists() && file.isFile()) {
//            System.out.println("文件"+file.getName()+"的大小是："+file.length());
            return file.length();
        }
        return -1;
    }

    /**
     * 创建文件（文件存在则不创建）
     */
    public static File createFile(String fileName, String folderName) {
        try {
            File file = new File(fileName);
            if (!StringUtils.isEmpty(folderName)) {
                File folderFile = new File(folderName);
                if (!folderFile.exists()) {
                    folderFile.mkdirs();
                }
            }
            if (file.createNewFile()) {
                return file;
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * 创建文件，判断文件是否已存在
     */
    public static File checkFileExist(String fileName, String folderName) {
        File file = new File(fileName);
        try {
            if (!file.exists() || !file.isFile()) {
                if (!StringUtils.isEmpty(folderName)) {
                    File folderFile = new File(folderName);
                    if (!folderFile.exists()) {
                        folderFile.mkdirs();
                    }
                }
                if (file.createNewFile()) {
                    return file;
                }
            }
            return file;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * 创建文件夹（文件存在则不创建）
     */
    public static File checkFolder(String folderName) {
        File folderFile = new File(folderName);
        try {
            if (!folderFile.exists()) {
                folderFile.mkdirs();
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return folderFile;
    }
    //删除文件夹
//param folderPath 文件夹完整绝对路径

    public static void delFolder(String folderPath) {
        try {
            delAllFile(folderPath); //删除完里面所有内容
            String filePath = folderPath;
            filePath = filePath;
            File myFilePath = new File(filePath);
            myFilePath.delete(); //删除空文件夹
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    //删除指定文件夹下所有文件
//param path 文件夹完整绝对路径
    public static boolean delAllFile(String path) {
        boolean flag = false;
        File file = new File(path);
        if (!file.exists() || !file.isDirectory()) {
            return flag;
        }
        String[] tempList = file.list();
        File temp;
        for (int i = 0; i < tempList.length; i++) {
            if (path.endsWith(File.separator)) {
                temp = new File(path + tempList[i]);
            } else {
                temp = new File(path + File.separator + tempList[i]);
            }
            if (temp.isFile()) {
                temp.delete();
            }
            if (temp.isDirectory()) {
                delAllFile(path + "/" + tempList[i]);//先删除文件夹里面的文件
                delFolder(path + "/" + tempList[i]);//再删除空文件夹
                flag = true;
            }
        }
        return flag;
    }

    public static File toFile(MultipartFile file) {
        File toFile = null;
        try {
            //if (!file.equals("") && file.getSize() > 0) {
            if (!file.equals("")) {
                InputStream ins = file.getInputStream();
                String foldername = ".." + File.separator;
                checkFolder(foldername);
                toFile = new File(foldername + file.getOriginalFilename());
                FileUtil.inputStreamToFile(ins, toFile);
                ins.close();
            }
            if (toFile.exists() && toFile.isFile() && toFile.canRead()) {
                return toFile;
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    public static File toFile(InputStream inputStream, String fileName) {
        File toFile;
        try {
            String foldername = ".." + File.separator;
            checkFolder(foldername);
            toFile = new File(foldername + fileName);
            FileUtil.inputStreamToFile(inputStream, toFile);
            inputStream.close();
            if (toFile.exists() && toFile.isFile() && toFile.canRead()) {
                return toFile;
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    public static void inputStreamToFile(InputStream ins, File file) {
        try {
            OutputStream os = new FileOutputStream(file);
            int bytesRead = 0;
            byte[] buffer = new byte[8192];
            while ((bytesRead = ins.read(buffer, 0, 8192)) != -1) {
                os.write(buffer, 0, bytesRead);
            }
            os.close();
            ins.close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    /**
     * 文件单例模式,减少系统资源消耗
     */
   /* public static File getSearchKeysFile() {
        if (searchKeysFile == null) {
            searchKeysFile = new File(Base.SEARCH_KEYS);
            if (!searchKeysFile.exists()){
                if (!searchKeysFile.getParentFile().exists()) {
                    searchKeysFile.getParentFile().mkdirs();
                }
                try {
                    searchKeysFile.createNewFile();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
        return searchKeysFile;
    }*/

    /**
     * BASE64解码成File文件
     * @param imgStr base64文件
     * @return
     */

    public static File Base64ToImage(String basePath, String fileName, String imgStr) { // 对字节数组字符串进行Base64解码并生成图片
        if (StringUtils.isEmpty(imgStr)) {
            // 图像数据为空
            throw BaseResponse.moreInfoError.error("文件上传失败");
        }
        try {
            File file = new File(basePath + File.separator + fileName);
            if (file.exists()) {
                file.delete();
            }
            file.createNewFile();

            // Base64解码
            byte[] b = Base64Util.decoder(imgStr);
            for (int i = 0; i < b.length; ++i) {
                // 调整异常数据
                if (b[i] < 0) {
                    b[i] += 256;
                }
            }
            OutputStream out = new FileOutputStream(file);
            out.write(b);
            out.flush();
            out.close();
            return file;
        } catch (Exception e) {
            throw BaseResponse.moreInfoError.error("文件上传失败");
        }

    }

}
