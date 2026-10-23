package com.ahaxt.competition.service.impl;

import com.ahaxt.competition.base.Base;
import com.ahaxt.competition.entity.base.BaseTreeInfo;
import com.ahaxt.competition.entity.db.BaseDepartment;
import com.ahaxt.competition.service.IImportAndExportService;
import com.ahaxt.competition.utils.EncodeUtil;
import com.ahaxt.competition.utils.FastJsonUtil;
import com.ahaxt.competition.utils.LogUtil;
import com.alibaba.fastjson.JSONArray;
import com.alibaba.fastjson.JSONObject;
import org.apache.poi.hssf.usermodel.*;
import org.apache.poi.hssf.util.HSSFColor;
import org.apache.poi.ss.usermodel.*;
import org.apache.poi.ss.util.CellRangeAddress;
import org.apache.poi.xssf.usermodel.XSSFWorkbook;
import org.springframework.context.ApplicationContext;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Sort;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.util.StringUtils;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletResponse;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.*;

@Service
@Transactional(rollbackFor = Exception.class)
public class ImportAndExportImpl extends Base implements IImportAndExportService {
    @Resource
    private ApplicationContext applicationContext;

    //region 一些private
    private HSSFCellStyle getCellStype( HSSFWorkbook workbook, int type) {
        HSSFCellStyle cellStyle = workbook.createCellStyle();
        HSSFFont hssfFont = workbook.createFont();
        switch (type) {
            case 1: //表头
                hssfFont.setBold(true);
                hssfFont.setColor(HSSFColor.HSSFColorPredefined.SEA_GREEN.getIndex());
                cellStyle.setFont(hssfFont);
                cellStyle.setFillPattern(FillPatternType.SOLID_FOREGROUND);
                cellStyle.setFillForegroundColor(HSSFColor.HSSFColorPredefined.LIGHT_CORNFLOWER_BLUE.getIndex());
                cellStyle.setAlignment(HorizontalAlignment.CENTER);
                cellStyle.setVerticalAlignment(VerticalAlignment.CENTER);
                cellStyle.setBorderBottom(BorderStyle.THIN);
                cellStyle.setBorderTop(BorderStyle.THIN);
                cellStyle.setBorderLeft(BorderStyle.THIN);
                cellStyle.setBorderRight(BorderStyle.THIN);
                break;
            case 2:
                hssfFont.setBold(true);
                hssfFont.setFontHeight((short)300);
                cellStyle.setFont(hssfFont);

                cellStyle.setAlignment(HorizontalAlignment.CENTER);
                cellStyle.setVerticalAlignment(VerticalAlignment.CENTER);
                break;
            case 3:
                cellStyle.setVerticalAlignment(VerticalAlignment.CENTER);
                break;
            default:
                cellStyle.setVerticalAlignment(VerticalAlignment.CENTER);
                cellStyle.setBorderBottom(BorderStyle.THIN);
                cellStyle.setBorderTop(BorderStyle.THIN);
                cellStyle.setBorderLeft(BorderStyle.THIN);
                cellStyle.setBorderRight(BorderStyle.THIN);
                break;
        }
        return cellStyle;
    }

    /**
     * 创建sheet并创建表头
     */
    private HSSFSheet createSheetandHeader(String key, HSSFWorkbook workbook, List<String> headerTitles, JSONArray nodes) {
        //创建一个sheet
        HSSFSheet hssfSheet = workbook.createSheet();
        //创建样式
        HSSFCellStyle titleStyle = getCellStype(workbook,1);
        //创建表头
        int rowNum = 0;
        if (key.equals("校级报名表")) {
            int lastCol = 3;
            CellRangeAddress rangeAddress = new CellRangeAddress(0, 0, 0, lastCol);
            //添加要合并地址到表格
            hssfSheet.addMergedRegion(rangeAddress);
            HSSFRow row1 = hssfSheet.createRow(rowNum);
            HSSFCell cell1 = row1.createCell(0);
            cell1.setCellValue(nodes.getJSONObject(0).getString("contestName")+"报名表");
            cell1.setCellStyle(getCellStype(workbook, 2));
            rowNum++;

            rangeAddress = new CellRangeAddress(1, 1, 0, lastCol);
            hssfSheet.addMergedRegion(rangeAddress);
            HSSFRow row2 = hssfSheet.createRow(rowNum);
            HSSFCell cell2 = row2.createCell(0);
            cell2.setCellValue("学校名称：" + nodes.getJSONObject(0).getString("schoolName"));
            cell2.setCellStyle(getCellStype(workbook, 3));
            rowNum++;

            HSSFRow row0 = hssfSheet.createRow(rowNum);
            int j = 0;
            HSSFCell cell01 = row0.createCell(j);
            cell01.setCellValue("组别/项目");
            cell01.setCellStyle(titleStyle);
            j++;
            HSSFCell cell02 = row0.createCell(j);
            cell02.setCellValue("队伍编码");
            cell02.setCellStyle(titleStyle);
            j++;
            HSSFCell cell03 = row0.createCell(j);
            cell03.setCellValue("教练");
            cell03.setCellStyle(titleStyle);
            j++;
            HSSFCell cell04 = row0.createCell(j);
            cell04.setCellValue("队员");
            cell04.setCellStyle(titleStyle);
        } else {
            HSSFRow row0 = hssfSheet.createRow(rowNum);
            for (int i = 0; i < headerTitles.size(); i++) {
                HSSFCell cell0 = row0.createCell(i);
                cell0.setCellValue(headerTitles.get(i));
                cell0.setCellStyle(titleStyle);
            }
        }
        return hssfSheet;
    }
    /**
     * excel表格根据内容自适应列宽
     * @author yukai
     * @param sheet
     * @param columnLength 列数
     */
    private void setSizeColumn(HSSFSheet sheet, int columnLength) {
        for (int columnNum = 0; columnNum <= columnLength; columnNum++) {
            int columnWidth = sheet.getColumnWidth(columnNum) / 256;
            for (int rowNum = 0; rowNum <= sheet.getLastRowNum(); rowNum++) {
                // 当前行未被使用过
                HSSFRow currentRow;
                if (sheet.getRow(rowNum) == null) {
                    currentRow = sheet.createRow(rowNum);
                } else {
                    currentRow = sheet.getRow(rowNum);
                }
                if (currentRow.getCell(columnNum) != null) {
                    HSSFCell currentCell = currentRow.getCell(columnNum);
                    if (currentCell.getCellType() == CellType.STRING) {
                        //兼容中文
                        int length = (currentCell.getStringCellValue().getBytes().length + currentCell.getStringCellValue().length()) / 2;
                        if (columnWidth < length) {
                            columnWidth = length;
                        }
                    }
                }
            }
            sheet.setColumnWidth(columnNum, columnWidth * 256);
        }
    }

    /**
     * 获取值之前判断是否有值
     * @author yukai
     * @param row
     * @param index
     */
    public boolean checkRow(Row row, int index){
        int num = 0;
        for (int i = 0;i < index; i++) {
            if (!StringUtils.isEmpty(row.getCell(i))){
                row.getCell(i).setCellType(CellType.STRING);
            }
            if ("".equals(row.getCell(i) == null ? "" : row.getCell(i).getStringCellValue().trim())){
                num++;
            }
        }
        return num == index;
    }

    private void dealExportInfo(String key, HSSFWorkbook wb, List<String> headerTitles, List<String> headerColNames, JSONArray nodes) {
        //设置表头
        HSSFSheet sheet = createSheetandHeader(key, wb, headerTitles, nodes);
        HSSFRow row;
        try {
            if (nodes.size() > 0) {
                List<Object> keys = new ArrayList<>();
                int j = 0;
                int span = 1;
                if (key.equals("校级报名表")) {
                    span = 3;
                    keys.add("smallContestTypeName");
                    keys.add("code");
                    keys.add("allTeacherNames");
                    keys.add("allStudentNames");
                } else {
                    // keys = Arrays.asList(nodes.getJSONObject(0).keySet().toArray());
                    keys = Arrays.asList(headerColNames.toArray());
                }
                HSSFCellStyle contentStyle = getCellStype(wb, 0);
                for (int i = 0; i < nodes.size(); i++) {
                    j = 0;
                    for (String colName : headerColNames) {
                       if (j == 0) {
                           row = sheet.createRow(i + span);
                       } else {
                           row = sheet.getRow(i + span);
                       }
                       String ss = "";
                       if (keys.contains(colName)) {
                            if (colName.contains("Date") || colName.contains("Time")) {
                                SimpleDateFormat format = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
                                ss = format.format(nodes.getJSONObject(i).getDate(colName));
                            } else if (colName.contains("isAudit")) {
                                switch (nodes.getJSONObject(i).getInteger(colName).intValue()) {
                                    case AUDIT_STATUS.SAVE:
                                        ss = AUDIT_STATUS.SAVENAME;
                                        break;
                                    case AUDIT_STATUS.SUBMIT:
                                        ss = AUDIT_STATUS.SUBMITNAME;
                                        break;
                                    case AUDIT_STATUS.PASS:
                                        ss = AUDIT_STATUS.PASSNAME;
                                        break;
                                    case AUDIT_STATUS.NOTPASS:
                                        ss = AUDIT_STATUS.NOTPASSNAME;
                                        break;
                                    case AUDIT_STATUS.BACK:
                                        ss = AUDIT_STATUS.BACKNAME;
                                        break;
                                }
                            } else {
                                ss = nodes.getJSONObject(i).getString(colName);
                            }
                            if (ss != null) {
                                row.createCell(j).setCellValue(ss);
                                row.getCell(j).setCellStyle(contentStyle);
                            } else {
                                row.createCell(j).setCellValue("无");
                                row.getCell(j).setCellStyle(contentStyle);
                            }
                       } else {
                           row.createCell(j).setCellValue("无");
                           row.getCell(j).setCellStyle(contentStyle);
                       }
                       j++;
                   }
                }
            }
        }catch (Exception ex) {
            LogUtil.error(logger, ex);
        }
        setSizeColumn(sheet, headerColNames.size());
    }
    //endregion



    @Override
    public Object exportInfo(String key, JSONArray nodes, JSONArray allTableColumns, JSONObject searchWords) {
        String keyWords = "";
        switch (key) {
            case "校级报名表": keyWords="ViewRegisterGroupDetail"; break;
            default: keyWords=key; break;
        }

        HSSFWorkbook wb = new HSSFWorkbook();
        JSONObject searchViewTableKey = new JSONObject();
        searchViewTableKey.put("tableName", keyWords);
        List<String> headerTitles = new ArrayList<>();
        List<String> headerColNames = new ArrayList<>();
        wb.createName().setNameName("导出数据");
        if (allTableColumns.size() != 0) {
            for (int i=0;i<allTableColumns.size();i++) {
                JSONObject obj = allTableColumns.getJSONObject(i);
                headerTitles.add(obj.get("showName").toString());
                headerColNames.add(obj.get("tableColumnName").toString());
            }
        } else {
            headerTitles.add("编码"); headerColNames.add("code");
            headerTitles.add("名称"); headerColNames.add("name");
            headerTitles.add("备注"); headerColNames.add("remarks");
        }
        //数据导出处理方法
        if (nodes.size()==0) {
            JSONObject searchKey = searchWords.getJSONObject("searchKey");
            JSONObject regKey = searchWords.getJSONObject("regKey");
            JSONObject andor = searchWords.getJSONObject("andor");
            Object resAll = ((Page<Object>)iCommonService.getSomeRecords(keyWords, searchKey, regKey,Sort.by(Sort.Direction.DESC, "id"),null,null,false, andor)).getContent();
            nodes = JSONObject.parseArray(FastJsonUtil.toJSONString(resAll));
            //判断下是否为树形结构
            if (nodes.size()>0) {
                Set<String> keys = nodes.getJSONObject(0).keySet();
                if (keys.contains("parentId")) {
                    ArrayList<Object> trees = (ArrayList<Object>) iDataTreeService.getTreeArrayList(keyWords, searchKey);
                    nodes.clear();
                    nodes = JSONObject.parseArray(FastJsonUtil.toJSONString(trees));
                }
            }
        }
        dealExportInfo(key, wb, headerTitles, headerColNames, nodes);
        return writeBrowser(wb, "ExportedData");
    }


    /**
     * 处理字段：数据库中为String，但是实际上为全数字，excel中默认的为数值型。进行转为String
     * @param row
     * @param i
     * @return String
     */
    public String dealWithStringRow(Row row , Integer i){
        String res = "";
        if (i>=0) {
            if (row.getCell(i) == null) {
                res = "";
            } else {
                if (row.getCell(i).getCellType() == CellType.NUMERIC) {
                    if (String.valueOf(row.getCell(i).getNumericCellValue()).indexOf("E") == -1) {
                        res = String.valueOf(row.getCell(i).getNumericCellValue());
                    } else {
                        res = new DecimalFormat("#").format(row.getCell(i).getNumericCellValue());
                    }
                } else if (row.getCell(i).getCellType() == CellType.STRING) {
                    res = row.getCell(i).getStringCellValue();
                } else if (row.getCell(i).getCellType() == CellType.BLANK) {
                    res = "";
                }
            }
        }
        return res;
    }

    /**
     * 写入树结构层级信息
     * @param row
     * @param curTreeInfo
     * @param allTreeInfo
     */
    public void setLevelInfo(HSSFRow row , BaseTreeInfo curTreeInfo, List<BaseTreeInfo> allTreeInfo){
        for(int i = curTreeInfo.getTheLevel(); i > 0; i--){
            row.createCell(i-1).setCellValue(curTreeInfo.getName());
            for(BaseTreeInfo baseTreeInfo : allTreeInfo){
                if(baseTreeInfo.getId().equals(curTreeInfo.getParentId())){
                    curTreeInfo = baseTreeInfo;
                    break;
                }
            }
        }
    }
    /**
     * 树转变为列表
     * @author yukai
     * @param treeInfo
     * @param list
     */
    public void treeToList(JSONObject treeInfo, List<JSONObject> list){
        list.add(treeInfo);
        if(treeInfo.getJSONArray("childList") != null) {
            for (JSONObject childTreeInfo : FastJsonUtil.toArray(treeInfo.getJSONArray("childList"), JSONObject.class)){
                treeToList(childTreeInfo, list);
            }
        }
    }

    /**
     * 判断excel文件版本
     * @author yukai
     * @param file
     */
    public Workbook judgeVersion(File file) {
        Workbook workbook = null;
        try {
            if (file.getPath().endsWith("xls")) {
                logger.info("这是2003版本");
                workbook = new HSSFWorkbook(new FileInputStream(file));
            } else if (file.getPath().endsWith("xlsx")) {
                workbook = new XSSFWorkbook(new FileInputStream(file));
                logger.info("这是2007版本");
            }
        } catch (IOException e) {
            e.printStackTrace();
        }

        return workbook;
    }

    /**
     * 文件写入浏览器
     * @author yukai
     * @param workbook
     * @param name
     */
    public Boolean writeBrowser(HSSFWorkbook workbook, String name) {
        ServletRequestAttributes attributes = (ServletRequestAttributes) RequestContextHolder.getRequestAttributes();
        HttpServletResponse response = attributes.getResponse();
        //写入浏览器
        try {
            String header  ="attachment;filename=" + URLEncoder.encode(name, StandardCharsets.UTF_8.toString()) + ".xls";
            response.setHeader("content-disposition", header);
            OutputStream outputStream = response.getOutputStream();
            workbook.write(outputStream);
            outputStream.flush();
            outputStream.close();
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }
}
