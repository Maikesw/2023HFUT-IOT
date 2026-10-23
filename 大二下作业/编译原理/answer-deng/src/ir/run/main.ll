define dso_local i32 @main() {
bb0:
        %r0 = alloca i32, align 4
        store i32 1, i32* %r0
        %r1 = load i32, i32* %r0
        br label %bb1
bb1:
        %r2 = phi i32 [ %r1,%bb0 ], [ 0,%bb2 ]
        ret i32 %r2
bb2:
        br label %bb1
}