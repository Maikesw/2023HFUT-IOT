package com.ahaxt.competition;

import com.ahaxt.competition.base.Base;
import org.junit.jupiter.api.Test;
import org.springframework.boot.test.context.SpringBootTest;

import java.io.File;

/**
 * @author yukai
 * @date: 2021/5/10
 */
@SpringBootTest(classes = RunApplication.class)
public class Demo extends Base {

    @Test
    void test() {
//        List<GasConcentration> list = new ArrayList<>();
//        GasConcentration gas1 = new GasConcentration(1, new BigInteger("51717"), true);
//        GasConcentration gas2 = new GasConcentration(2, new BigInteger("51718"), false);
//        list.add(gas1);
//        list.add(gas2);
//        dailyValueDeal("MLJ0033", list);
//        CertificateDto certificateDto = new CertificateDto("11", "11","11","11","11",1212.0,"11","222","222");
//        File file = PdfUtils.download("certificatePdf.ftl", certificateDto);
//        System.out.println(file.toString());
    }

//    public void dailyValueDeal(String code, List<GasConcentration> gasList) {
//        JSONObject searchKey = new JSONObject();
//        Map<String, String> regMap = new HashMap<>();
//        searchKey.put("code", code);
//        List<TblMonitorInfo> monitorInfos = (List<TblMonitorInfo>) iCommonService.getSomeRecords(MonitorInfo, searchKey);
//        TblMonitorInfo monitorInfo = monitorInfos.get(0);
//        List<Integer> channel_numbers = gasList.stream().map(GasConcentration::getChannel_number).collect(Collectors.toList());
//        searchKey.clear();
//        searchKey.put("remarks", GeneralUtil.iteratorToString(channel_numbers));
//        searchKey.put("monitorInfoId", monitorInfo.getId());
//        regMap.put("remarks", IN);
//        List<TblMonitorInfoAttribute> attributes = (List<TblMonitorInfoAttribute>) iCommonService.getSomeRecords(MonitorInfoAttribute, searchKey, regMap);
//        Map<String, Integer> remarksToId = attributes.stream().collect(Collectors.toMap(TblMonitorInfoAttribute::getRemarks, TblMonitorInfoAttribute::getId));
//        for (GasConcentration gas : gasList) {
//            TblMonitorDailyValue dailyValue = new TblMonitorDailyValue();
//            dailyValue.setMonitorAttributeId(remarksToId.get(String.valueOf(gas.getChannel_number())));
//            dailyValue.setNumValue(gas.getConcentration().intValue());
//            dailyValue.setBoolValue(gas.getAlarm_flag());
//            dailyValue.setStrValue("channel_number=" + gas.getChannel_number() + ", concentration=" + gas.getConcentration() + ", alarm_flag=" + gas.getAlarm_flag());
//            dailyValue.setUploadTime(new Date());
//        }
//    }
}
