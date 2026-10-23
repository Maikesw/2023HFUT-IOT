# ============================================
# FPGA快速上板烧录脚本
# 使用方法：在Vivado TCL Console中执行 source program_fpga.tcl
# ============================================

# 打开项目
puts "正在打开项目..."
open_project MultiTCPU.xpr

# 检查bitstream文件是否存在
set bitstream_file "MultiTCPU.runs/impl_1/CPU.bit"
if {![file exists $bitstream_file]} {
    puts "警告：Bitstream文件不存在，正在生成..."
    puts "正在运行实现..."
    launch_runs impl_1 -to_step write_bitstream -jobs 4
    wait_on_run impl_1
    
    # 检查是否成功生成
    if {![file exists $bitstream_file]} {
        puts "错误：Bitstream生成失败！请检查实现日志。"
        return
    }
    puts "Bitstream生成成功！"
} else {
    puts "找到Bitstream文件：$bitstream_file"
}

# 打开硬件管理器
puts "正在连接硬件..."
open_hw

# 连接硬件服务器
if {[catch {connect_hw_server} result]} {
    puts "错误：无法连接硬件服务器 - $result"
    puts "请确保："
    puts "  1. 开发板已通过USB连接到电脑"
    puts "  2. 开发板已上电"
    puts "  3. USB驱动已正确安装"
    return
}

# 打开硬件目标
if {[catch {open_hw_target} result]} {
    puts "错误：无法打开硬件目标 - $result"
    puts "请检查硬件连接"
    return
}

# 获取设备列表
set devices [get_hw_devices]
if {[llength $devices] == 0} {
    puts "错误：未检测到硬件设备！"
    return
}

puts "检测到设备：$devices"

# 选择目标设备（xc7a35t_0）
set target_device [lindex [get_hw_devices xc7a35t_0] 0]
if {$target_device == ""} {
    puts "警告：未找到xc7a35t_0设备，使用第一个可用设备"
    set target_device [lindex $devices 0]
}

puts "使用设备：$target_device"

# 设置当前设备
current_hw_device $target_device

# 刷新设备
puts "正在刷新设备..."
refresh_hw_device -update_hw_probes false $target_device

# 设置bitstream文件
puts "设置bitstream文件：$bitstream_file"
set_property PROGRAM.FILE [file normalize $bitstream_file] $target_device

# 烧录bitstream
puts "正在烧录bitstream到FPGA..."
if {[catch {program_hw_devices $target_device} result]} {
    puts "错误：烧录失败 - $result"
    return
}

puts "============================================"
puts "烧录成功！"
puts "============================================"
puts "现在FPGA应该正在运行您的设计。"
puts "观察数码管显示（应该显示ID阶段的指令）。"
puts "按RST按钮（P15）可以单步执行CPU指令。"
puts "============================================"

# 刷新设备（可选，用于更新调试探针）
refresh_hw_device $target_device


