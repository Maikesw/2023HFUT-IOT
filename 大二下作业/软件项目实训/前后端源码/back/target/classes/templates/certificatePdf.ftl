<!DOCTYPE html>
<html>
<head lang="en">
    <meta charset="utf-8"/>
    <title>PDF模板</title>
    <style>
        @page {
            size: 297mm 210mm; /*设置纸张大小:A4(210mm 297mm)、A3(297mm 420mm) 横向则反过来*/
            margin: 0.25in;
            padding: 1em;
        }

        #backImg {
            position: absolute;
            top: 0;
            right: 0;
            width: 1000px;
            height: 700px;
            background: url('https://axt-public.oss-cn-hangzhou.aliyuncs.com/competition/img/bk.jpg') no-repeat;
            background-size: 100% 100%;
        }
        .tbl {
            position: absolute;
            top: 230px;
            left: 100px;
            width: 800px;
            height: 450px;
        }
        .font1 {
            text-align: center;
            font-face: "FZHei-B01S";
            color: blue;
            font-size: 28pt;
        }

        .font2 {
            text-align: center;
            font-face: "FZHei-B01S";
            color: red;
            font-size: 35pt;
        }

        .font3 {
            color: red;
            font-size: 16pt;
        }

        .font4 {
            color: black;
            font-size: 16pt;
            /*text-decoration: underline;*/
        }
    </style>
</head>
<body style="font-family: 'FangSong_GB2312'">
<div id="backImg">
    <table class="tbl">
        <tr>
            <td class="font1">
                <div style="width: 100%; white-space:normal;word-wrap:break-word;word-break:break-all;">
                    ${contestName}
                </div>
            </td>
        </tr>
        <tr>
            <td class="font1">${smallContestTypeName}</td>
        </tr>
        <tr>
            <td class="font2">${groupResult}</td>
        </tr>
        <tr style="height: 40px">
            <td></td>
        </tr>
        <tr>
            <td style="align: center">
                <table>
                    <tr>
                        <td style="width: 50px"></td>
                        <td style="width: 350px"><span class="font3">学校名称：</span><span
                                    class="font4">${schoolName}</span></td>
                        <td><span class="font3">证书编号：</span><span class="font4">${code}</span></td>
                    </tr>
                    <tr>
                        <td style="width: 50px"></td>
                        <td style="width: 350px"><span class="font3">指导教练：</span><span
                                    class="font4">${allTeacherNames}</span></td>
                        <td><span class="font3">参赛队员：</span><span class="font4">${allStudentNames}</span></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 20px"></td>
        </tr>
        <tr>
            <td style="align: center">
                <table>
                    <tr>
                        <td style="width: 600px"></td>
                        <td style="width: 200px; align: center" class="font3">${unit}</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td style="width: 600px"></td>
                        <td style="width: 200px; align: center" class="font3">${date}</td>
                        <td></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="height: 80px"></td>
        </tr>
    </table>
</div>
</body>
</html>