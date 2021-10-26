import React, { useContext } from 'react';
import { Button, Popconfirm, notification, Tooltip } from 'antd';
import { DeleteOutlined, SmileOutlined } from '@ant-design/icons';
import 'antd/dist/antd.css';

import { ProposalsContext } from '../../Context/pendingRequirements';

const ProposalRemover = ({ record }) => {

    const { RemovePendingRequirement } = useContext(ProposalsContext);

    const openNotification = () => {
        notification.open({
            message: 'Pomyślnie usunięto zgłoszenie',
            description:
                'Zgłoszenie nie zostanie już zweryfikowane przez właściciela projektu.',
            icon: <SmileOutlined style={{ color: '#108ee9' }} rotate={180} />,
        });
    };

    const handleDelete = async (oid) => {
        RemovePendingRequirement({ pendingOid: oid });
        openNotification();
    };

    return (
        <Popconfirm title="Czy na pewno?" okText="Tak" cancelText="Nie" onConfirm={() => handleDelete(record.oid)}>
            <Tooltip title="Usuń">
                <Button size="small" shape="circle" icon={<DeleteOutlined />} />
            </Tooltip>
        </Popconfirm>
    )
}

export default ProposalRemover;