# 2023HFUT-IOT

> 合肥工业大学 2023 级 物联网工程 专业 · 本科课程资料合集

本仓库收录了本人在合肥工业大学物联网工程专业本科期间整理的全部课程资料——课件、实验报告、课程设计、代码工程与参考资料，按学期归档，供学弟学妹参考。

<p align="left">
  <img alt="HFUT" src="https://img.shields.io/badge/学校-合肥工业大学-1a5fb4">
  <img alt="Major" src="https://img.shields.io/badge/专业-物联网工程-26a269">
  <img alt="Year" src="https://img.shields.io/badge/年级-2023级-c6465f">
  <img alt="LFS" src="https://img.shields.io/badge/大文件-Git%20LFS-e5a50a">
</p>

---

## 目录结构

```
2023HFUT-IOT/
├── 大一上作业/        C 语言程序设计、大作业
├── 大一下作业/        面向对象 / 数据结构、GUI 课程设计
├── 大二上作业/        数字逻辑、计组、嵌入式等
├── 大二下作业/        机器学习、编译原理、大物实验等
├── 大三上/            操作系统、EDA 实训、系统综合设计等
├── 大三下及大四/      数据库、软件工程等
├── .gitattributes     大文件 Git LFS 登记表
├── .gitignore         忽略规则
└── README.md
```

## 各学期内容

| 目录 | 文件数 | 体积 | 主要内容 |
| :-- | --: | --: | :-- |
| `大一上作业/` | 74 | 47.5 MB | C 语言程序设计实验、大程序设计作业 |
| `大一下作业/` | 345 | 158.7 MB | 面向对象程序设计、学生管理系统 GUI 课设 |
| `大二上作业/` | 624 | 296.8 MB | 数字逻辑、计算机组成、嵌入式等课程实验 |
| `大二下作业/` | 4306 | 447.3 MB | 机器学习大作业、编译原理课设、大学物理实验 |
| `大三上/` | 2477 | 662.8 MB | 操作系统、EDA 实训全套资料、系统综合设计 |
| `大三下及大四/` | 41 | 31.6 MB | 数据库原理与上机、软件工程 |

> 文件数与体积为当前工作区统计，会随更新变化。

## 使用方法

### 克隆仓库

仓库使用 [Git LFS](https://git-lfs.com/) 管理 10MB 以上的大文件，**请先安装 Git LFS**，否则大文件只会得到一个几百字节的指针文件。

```bash
# 1. 安装并初始化 Git LFS（只需一次）
git lfs install

# 2. 克隆（大文件会在克隆后自动拉取）
git clone https://github.com/Maikesw/2023HFUT-IOT.git

# 3. 若克隆时跳过了大文件，手动补拉
cd 2023HFUT-IOT
git lfs pull
```

### 只下载单个文件或文件夹

不想克隆整个仓库时，可以使用以下任一方式：

- **浏览器直接下载**：在 GitHub 页面点开文件 → 右上角 Download raw file；大文件需点 "Download" 按钮（走 LFS 的 media 链接）。
- **在线目录浏览**：[DownGit](https://minhaskamal.github.io/DownGit/) 或 [download-directory](https://download-directory.github.io/)，粘贴子目录链接即可打包下载。
- **稀疏检出**（只拉某个学期，推荐）：

```bash
git clone --filter=blob:none --sparse https://github.com/Maikesw/2023HFUT-IOT.git
cd 2023HFUT-IOT
git sparse-checkout set 大三上
```

### 常见问题

**Q：下载下来是个几百字节的文本文件，写着 `version https://git-lfs.github.com/spec/v1`？**

A：那是 LFS 指针，说明本地没装 Git LFS。执行 `git lfs install && git lfs pull` 即可还原。

**Q：`git lfs pull` 太慢或中断？**

A：可以分批拉取，例如 `git lfs pull --include="大三上/**"`。

## 关于大文件

仓库里 10MB 以上的文件（视频、压缩包、大 PDF、数据集等）都通过 Git LFS 存储，清单逐条记录在 `.gitattributes` 中。

贡献或补充资料时，若新增文件超过 10MB，请用 LFS 追踪：

```bash
git lfs track "路径/文件名"
git add .gitattributes 路径/文件名
git commit -m "添加 xxx"
```

只要对象已经进入历史，只改 `.gitattributes` 是没用的，需要 `git lfs migrate import --include="路径"` 重写历史；公开仓库上这样做需要强制推送，请务必确认后果。

## 贡献

欢迎补充、纠错或提交你整理的资料：

1. Fork 本仓库
2. 新建分支 `git checkout -b add/课程名`
3. 提交改动（大文件请走 LFS）
4. 发起 Pull Request，说明资料归属的课程与学期

也可以直接提 [Issue](https://github.com/Maikesw/2023HFUT-IOT/issues) 反馈缺失或错误的文件。

## 说明

- 资料为个人整理，**不保证内容完全正确**，仅作学习参考；课程要求以当年任课教师的通知为准。
- 课件、教材、试卷等原始材料的版权归合肥工业大学及各任课教师所有，仅限个人学习交流使用，请勿用于商业用途。
- **请遵守学术诚信**：参考可以，直接抄袭作业或实验报告可能带来严重后果。
- 如涉及侵权，请提交 Issue，我会尽快处理。

---

<p align="center">
  <sub>如果这份资料帮到了你，欢迎点亮 ⭐ Star</sub>
</p>
