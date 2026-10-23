package com.ahaxt.competition.utils;

import org.apache.commons.collections.ListUtils;
import org.springframework.util.CollectionUtils;

import java.util.*;

/**
 * @author hongzhangming
 */
@SuppressWarnings("unchecked")
public class CollectionUtil extends CollectionUtils {

    public static <T> List<T> asList(T... obj) {
        return Arrays.asList(obj);
    }

    public static <T> Set<T> asSet(T... obj) {
        return new HashSet<>(Arrays.asList(obj));
    }

    public static <T> List<T> asList(Collection<T> collection) {
        return new ArrayList<>(collection);
    }

    public static <T> Set<T> asSet(Collection<T> collection) {
        return new HashSet<>(collection);
    }

    /**
     * 将list 的 两个下标位置元素换位
     * @param list
     * @param index
     * @param index1
     */
    public static <E> List<E> conversion(List<E> list, int index, int index1) {
        if (0 <= index && index < list.size() && 0 <= index1 && index1 < list.size()) {
            list.set(index1, list.set(index, list.get(index1)));
        } else {
            throw new ArrayIndexOutOfBoundsException();
        }
        return list;
    }

    public static <T> List<T> sum(List<T> list1, List<T> list2) {
        return ListUtils.sum(list1, list2);
    }
}
