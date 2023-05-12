// TagData.h

#pragma once

#include <map>
#include <string>
#include <cinttypes>
#include <vector>

namespace MineMP {
    static class TagData {

    public:
        struct TagDataItem {
            // Static Types
            typedef const char* str_t;
            typedef const char* const readonlystr_t;
            typedef str_t* p_readonlystr_t;
            typedef TagDataItem* p_ThisPtr_t;
            typedef void* data_t;
            typedef const data_t readonlydata_t;
            typedef const data_t* const p_readonlydata_t;
            typedef enum {
                Null = -0x01,
                End,        // uint8_t
                Byte,       // uint8_t
                Short,      // uint16_t
                Int,        // uint32_t
                Long,       // uint64_t
                Float,      // float_t
                Double,     // double_t
                ByteArray,  // std::vector<uint8_t>
                String,     // std::string
                List,       // std::vector<void*>
                Compound,   // TagDataItem
                IntArray,   // std::vector<uint32_t>
                LongArray   // std::vector<uint64_t>
            }types;

            // Functions
        public:
            types GetType(p_readonlystr_t p_strDataName);

            static void Write(p_ThisPtr_t p_itemThisPtr, p_readonlystr_t p_strDataName, types enumDataType, p_readonlydata_t p_pvoidData);
            inline void Write(p_readonlystr_t p_strDataName, types enumDataType, p_readonlydata_t p_pvoidData);

            inline static void Delete(p_ThisPtr_t p_itemThisPtr, p_readonlystr_t p_strDataName);
            inline void Delete(p_readonlystr_t p_strDataName);

            inline p_readonlydata_t Read(p_readonlystr_t p_strDataName);

        private:
            std::map<readonlystr_t, data_t> mapMapData;
            std::map<readonlystr_t, types> mapMapDataTypesMapping;
        };


        struct Tag {

            Tag() {}
            ~Tag() {}
        };

    };
}

