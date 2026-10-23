package com.ahaxt.competition.utils;

import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.stereotype.Component;
import org.springframework.util.StringUtils;

import javax.annotation.Resource;
import java.util.*;
import java.util.concurrent.TimeUnit;

/**
 * @author hongzhangming
 */
@Component
public final class RedisUtil {
    @Resource
    private RedisTemplate<String, Object> redisTemplate;

    /** ============================= common ============================ **/
    /**
     * 指定缓存失效时间
     * @param key
     * @param timeoutSecond 时间(秒)
     * @return
     */
    public Boolean expire(String key, long timeoutSecond) {
        try {
            if (timeoutSecond > 0) {
                return redisTemplate.expire(key, timeoutSecond, TimeUnit.SECONDS);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    /**
     * 根据key 获取过期时间
     * @param key 键 不能为null
     * @return 时间(秒) 返回0代表为永久有效
     */
    public Long getExpire(String key) {
        return redisTemplate.getExpire(key, TimeUnit.SECONDS);
    }

    /**
     * has key
     * @param key
     * @return
     */
    public Boolean hasKey(String key) {
        try {
            return redisTemplate.hasKey(key);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    /**
     * delete keys
     * @param keys
     */
    public Boolean delete(String... keys) {
        try {
            if (keys != null && keys.length > 0) {
                if (keys.length == 1) {
                    return redisTemplate.delete(keys[0]);
                } else {
                    return redisTemplate.delete(Arrays.asList(keys)) != 0;
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    /** ============================ String or object ============================= **/
    /**
     * get value of key
     */
    public Object get(String key) {
        try {
            if (!StringUtils.isEmpty(key)) {
                return redisTemplate.opsForValue().get(key);
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * set key-value
     * @param key
     * @param value
     * @return
     */
    public boolean set(String key, Object value) {
        try {
            redisTemplate.opsForValue().set(key, value);
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
        return true;
    }

    /**
     * set key-value and timeout : if second <= 0 ,key will not invalid
     * @param key
     * @param value
     * @param timeoutSecond (second)
     * @return
     */
    public boolean set(String key, Object value, long timeoutSecond) {
        try {
            if (timeoutSecond > 0) {
                redisTemplate.opsForValue().set(key, value, timeoutSecond, TimeUnit.SECONDS);
            } else {
                return set(key, value);
            }
            return true;
        } catch (Exception e) {
            e.printStackTrace();
        }
        return false;
    }

    /**
     * Increment an integer value stored as string value under {@code key} by {@code delta}.
     * @param key   must not be {@literal null}.
     * @param delta
     * @return {@literal null} when used in pipeline / transaction.
     * @see <a href="http://redis.io/commands/incrby">Redis Documentation: INCRBY</a>
     */
    public Long increment(String key, long delta) {
        return redisTemplate.opsForValue().increment(key, delta);
    }
    // ================================Map=================================

    /**
     * get hash
     * @param key
     * @return
     */
    public Map<Object, Object> hGet(String key) {
        return redisTemplate.opsForHash().entries(key);
    }

    /**
     * Determine if given hash {@code hashKey} exists.
     * @param key     must not be {@literal null}.
     * @param hashKey must not be {@literal null}.
     * @return {@literal null} when used in pipeline / transaction.
     */
    public Boolean hHasKey(String key, String hashKey) {
        return redisTemplate.opsForHash().hasKey(key, hashKey);
    }

    /**
     * Get Hash Value
     * @param key     must not be null
     * @param hashKey must not be null
     * @return
     */
    public Object hvGet(String key, String hashKey) {
        return redisTemplate.opsForHash().get(key, hashKey);
    }

    /**
     * set many item of hash
     * @param key
     * @param map
     * @return
     */
    public boolean hSet(String key, Map<String, Object> map) {
        try {
            redisTemplate.opsForHash().putAll(key, map);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * set many item of hash and set timeout
     * @param key
     * @param map
     * @param time (second)
     * @return
     */
    public boolean hSet(String key, Map<String, Object> map, long time) {
        try {
            redisTemplate.opsForHash().putAll(key, map);
            if (time > 0) {
                expire(key, time);
            }
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 向一张hash表中放入数据,如果不存在将创建
     * @param key
     * @param hashKey
     * @param value
     * @return
     */
    public boolean hvSet(String key, String hashKey, Object value) {
        try {
            redisTemplate.opsForHash().put(key, hashKey, value);
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
        return true;
    }

    /**
     * 向一张hash表中放入数据,如果不存在将创建
     * @param key
     * @param hashKey
     * @param value
     * @param timeoutSecond 时间(秒) 注意:如果已存在的hash表有时间,这里将会替换原有的时间
     * @return true 成功 false失败
     */
    public boolean hvSet(String key, String hashKey, Object value, long timeoutSecond) {
        try {
            redisTemplate.opsForHash().put(key, hashKey, value);
            if (timeoutSecond > 0) {
                return expire(key, timeoutSecond);
            }
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
        return true;
    }

    /**
     * 删除hash表中的值
     * @param key      键 不能为null
     * @param hashKeyS 项 可以使多个 不能为null
     */
    @SuppressWarnings("all")
    public Long hVDelete(String key, Object... hashKeyS) {
        return redisTemplate.opsForHash().delete(key, hashKeyS);
    }

    /**
     * Increment {@code value} of a hash {@code hashKey} by the given {@code delta}.
     * @param key     must not be {@literal null}.
     * @param hashKey must not be {@literal null}.
     * @param delta
     * @return {@literal null} when used in pipeline / transaction.
     */
    public Double hvIncrement(String key, String hashKey, double delta) {
        return redisTemplate.opsForHash().increment(key, hashKey, delta);
    }
    // ============================set=============================

    /**
     * 根据key获取Set中的所有值
     * @param key 键
     * @return
     */
    public Set<Object> sGet(String key) {
        return redisTemplate.opsForSet().members(key);
    }

    /**
     * 根据value从一个set中查询,是否存在
     * @param key   键
     * @param value 值
     * @return true 存在 false不存在
     */
    public Boolean sHasKey(String key, Object value) {
        return redisTemplate.opsForSet().isMember(key, value);
    }

    /**
     * 将数据放入set缓存
     * @param key    键
     * @param values 值 可以是多个
     * @return 成功个数
     */
    public Long sSet(String key, Object... values) {
        return redisTemplate.opsForSet().add(key, values);
    }

    /**
     * 将set数据放入缓存 (不需要判断 key 是否存在)
     * @param key           键
     * @param timeoutSecond 时间(秒)
     * @param values        值 可以是多个
     * @return 成功个数
     */
    public Long sSetAndTime(String key, long timeoutSecond, Object... values) {
        try {
            if (timeoutSecond > 0) {
                if (expire(key, timeoutSecond)) {
                    return -1L;
                }
            }
            return redisTemplate.opsForSet().add(key, values);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0L;
    }

    /**
     * 获取set缓存的长度
     * @param key 键
     * @return
     */
    public Long sGetSize(String key) {
        try {
            return redisTemplate.opsForSet().size(key);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0L;
    }

    /**
     * 移除值为value的
     * @param key    键
     * @param values 值 可以是多个
     * @return 移除的个数
     */
    public Long setRemove(String key, Object... values) {
        try {
            return redisTemplate.opsForSet().remove(key, values);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0L;
    }

    /** ===============================list================================= **/
    /**
     * 获取list缓存的内容
     * @param key   键
     * @param start 开始
     * @param end   结束 0 到 -1代表所有值
     * @return
     */
    public List<Object> lGet(String key, long start, long end) {
        try {
            return redisTemplate.opsForList().range(key, start, end);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * 获取list缓存的长度
     * @param key 键
     * @return
     */
    public Long lGetSize(String key) {
        try {
            return redisTemplate.opsForList().size(key);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return 0L;
    }

    /**
     * 通过索引 获取list中的值
     * @param key   键
     * @param index 索引 index>=0时， 0 表头，1 第二个元素，依次类推；index<0时，-1，表尾，-2倒数第二个元素，依次类推
     * @return
     */
    public Object lGetIndex(String key, long index) {
        try {
            return redisTemplate.opsForList().index(key, index);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return null;
    }

    /**
     * 将list放入缓存
     * @param key   键
     * @param value 值
     * @return
     */
    public boolean lSet(String key, Object value) {
        try {
            redisTemplate.opsForList().rightPush(key, value);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 将list放入缓存
     * @param key     键
     * @param value   值
     * @param timeout 时间(秒)
     * @return
     */
    public boolean lSet(String key, Object value, long timeout) {
        try {
            redisTemplate.opsForList().rightPush(key, value);
            if (timeout > 0) {
                expire(key, timeout);
            }
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 将collection放入缓存
     * @param key        键
     * @param collection 值
     * @return
     */
    public boolean lSet(String key, Collection<Object> collection) {
        try {
            redisTemplate.opsForList().rightPushAll(key, collection);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 将list放入缓存
     * @param key        键
     * @param collection 值
     * @param timeout    时间(秒)
     * @return
     */
    public boolean lSet(String key, Collection<Object> collection, long timeout) {
        try {
            redisTemplate.opsForList().rightPushAll(key, collection);
            if (timeout > 0) {
                expire(key, timeout);
            }
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 根据索引修改list中的某条数据
     * @param key   键
     * @param index 索引
     * @param value 值
     * @return
     */
    public boolean lUpdateIndex(String key, long index, Object value) {
        try {
            redisTemplate.opsForList().set(key, index, value);
            return true;
        } catch (Exception e) {
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 移除N个值为value
     * @param key   键
     * @param count 移除多少个
     * @param value 值
     * @return 移除的个数
     */
    public long lRemove(String key, long count, Object value) {
        try {
            return redisTemplate.opsForList().remove(key, count, value);
        } catch (Exception e) {
            e.printStackTrace();
            return 0;
        }
    }
}