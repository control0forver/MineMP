// TagData.cpp

#include "TagData.h"

namespace MineMP {
    TagData::TagDataItem::types TagData::TagDataItem::GetType(TagData::TagDataItem::p_readonlystr_t p_strDataName)
    {
        return this->mapMapDataTypesMapping[*p_strDataName];
    }

    void TagData::TagDataItem::Write(TagData::TagDataItem::p_ThisPtr_t p_itemThisPtr, TagData::TagDataItem::p_readonlystr_t p_strDataName, TagData::TagDataItem::types enumDataType, TagData::TagDataItem::p_readonlydata_t p_pvoidData)
    {
        if (!p_itemThisPtr || !p_strDataName || !*p_strDataName)
            return;

        // Delete
        if (enumDataType == types::Null) {
            p_itemThisPtr->mapMapData.erase(*p_strDataName);
            p_itemThisPtr->mapMapDataTypesMapping.erase(*p_strDataName);
            delete (*p_pvoidData);

            return;
        }

        data_t data;
        auto funcAdd = ([&]() {
            p_itemThisPtr->mapMapData[*p_strDataName] = data;
            p_itemThisPtr->mapMapDataTypesMapping[*p_strDataName] = enumDataType;
            });

        if (types::End == enumDataType) {
            data = new uint8_t(*(uint8_t*)(*p_pvoidData));
        }
        if (types::Byte == enumDataType) {
            data = new uint8_t(*(uint8_t*)(*p_pvoidData));
        }
        if (types::Short == enumDataType) {
            data = new uint16_t(*(uint16_t*)(*p_pvoidData));
        }
        if (types::Int == enumDataType) {
            data = new uint32_t(*(uint32_t*)(*p_pvoidData));
        }
        if (types::Long == enumDataType) {
            data = new uint64_t(*(uint64_t*)(*p_pvoidData));
        }
        if (types::Float == enumDataType) {
            data = new float_t(*(float_t*)(*p_pvoidData));
        }
        if (types::Double == enumDataType) {
            data = new double_t(*(double_t*)(*p_pvoidData));
        }
        if (types::ByteArray == enumDataType) {
            data = new std::vector<uint8_t>(*(std::vector<uint8_t>*)(*p_pvoidData));
        }
        if (types::String == enumDataType) {
            data = new std::string(*(std::string*)(*p_pvoidData));
        }
        if (types::List == enumDataType) {
            data = new std::vector<void*>(*(std::vector<void*>*)(*p_pvoidData));
        }
        if (types::Compound == enumDataType) {
            data = new TagDataItem(*(TagDataItem*)(*p_pvoidData));
        }
        if (types::IntArray == enumDataType) {
            data = new std::vector<uint32_t>(*(std::vector<uint32_t>*)(*p_pvoidData));
        }
        if (types::LongArray == enumDataType) {
            data = new std::vector<uint64_t>(*(std::vector<uint64_t>*)(*p_pvoidData));
        }

        funcAdd();
        goto lab_return;

    lab_return:
        return;
    }
    inline void TagData::TagDataItem::Write(TagData::TagDataItem::p_readonlystr_t p_strDataName, TagData::TagDataItem::types enumDataType, TagData::TagDataItem::p_readonlydata_t p_pvoidData)
    {
        return Write(this, p_strDataName, enumDataType, p_pvoidData);
    }

    inline void TagData::TagDataItem::Delete(p_ThisPtr_t p_itemThisPtr, p_readonlystr_t p_strDataName)
    {
        return Write(p_itemThisPtr, p_strDataName, types::Null, nullptr);
    }
    inline void TagData::TagDataItem::Delete(p_readonlystr_t p_strDataName)
    {
        return Delete(this, p_strDataName);
    }

    inline TagData::TagDataItem::p_readonlydata_t TagData::TagDataItem::Read(p_readonlystr_t p_strDataName)
    {
        if (mapMapData.count(*p_strDataName))
            return &mapMapData[*p_strDataName];
        return nullptr;
    }
}
